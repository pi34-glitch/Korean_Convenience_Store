using KoreanStoreApi.Data;
using KoreanStoreApi.DTOs;
using KoreanStoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoreanStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public VentasController(ApplicationDbContext context) => _context = context;

        // GET: api/ventas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VentaDto>>> GetAll()
        {
            var ventas = await _context.Ventas
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
                .Select(v => new VentaDto
                {
                    Id_venta = v.Id_venta,
                    Id_user = v.Id_user,
                    Fecha_venta = v.Fecha_venta,
                    Total = v.Total,
                    Metodo_pago = v.Metodo_pago.ToString(),
                    Detalles = v.Detalles.Select(d => new DetalleVentaDto
                    {
                        Id_detalle = d.Id_detalle,
                        Id_venta = d.Id_venta,
                        Id_producto = d.Id_producto,
                        Cantidad_o_gramos = d.Cantidad_o_gramos,
                        Precio_aplicado = d.Precio_aplicado,
                        Subtotal = d.Subtotal,
                        NombreProducto = d.Producto != null ? d.Producto.Nombre : null
                    }).ToList()
                })
                .ToListAsync();
            return Ok(ventas);
        }

        // POST: api/ventas
        // Procesa la venta y descuenta el stock de forma atómica en PostgreSQL
        [HttpPost]
        public async Task<ActionResult<VentaDto>> Create([FromBody] VentaDto dto)
        {
            if (dto == null || dto.Detalles == null || !dto.Detalles.Any())
                return BadRequest(new { mensaje = "La venta debe contener al menos un detalle." });

            if (!Enum.TryParse<MetodoPago>(dto.Metodo_pago, out var metodo))
                return BadRequest(new { mensaje = "Método de pago inválido" });

            // Inicio de transacción atómica
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var venta = new Venta
                {
                    Id_user = dto.Id_user,
                    Fecha_venta = DateTime.UtcNow,
                    Total = dto.Total,
                    Metodo_pago = metodo,
                    Detalles = new List<DetalleVenta>()
                };

                decimal totalCalculado = 0;

                foreach (var d in dto.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(d.Id_producto);

                    if (producto == null)
                    {
                        await transaction.RollbackAsync();
                        return NotFound(new { mensaje = $"Producto ID {d.Id_producto} no encontrado." });
                    }

                    // Validación de disponibilidad de stock
                    if (producto.Stock_actual < d.Cantidad_o_gramos)
                    {
                        await transaction.RollbackAsync();
                        return BadRequest(new { mensaje = $"Stock insuficiente para '{producto.Nombre}'. Disponible: {producto.Stock_actual}" });
                    }

                    // Descuento atómico de existencias
                    producto.Stock_actual -= d.Cantidad_o_gramos;

                    decimal precioUsar = d.Precio_aplicado > 0 ? d.Precio_aplicado : producto.Precio_unitario;
                    decimal subtotalUsar = d.Cantidad_o_gramos * precioUsar;

                    totalCalculado += subtotalUsar;

                    venta.Detalles.Add(new DetalleVenta
                    {
                        Id_producto = d.Id_producto,
                        Cantidad_o_gramos = d.Cantidad_o_gramos,
                        Precio_aplicado = precioUsar,
                        Subtotal = subtotalUsar
                    });
                }

                venta.Total = totalCalculado;

                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                // Confirmación exitosa de la transacción
                await transaction.CommitAsync();

                dto.Id_venta = venta.Id_venta;
                dto.Total = venta.Total;
                return Ok(dto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { mensaje = $"Error al procesar la venta: {ex.Message}" });
            }
        }
    }
}