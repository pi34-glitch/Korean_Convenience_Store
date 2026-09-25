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
        // Requerimiento Sprint 2: Transacción atómica y descuento automático de inventario
        [HttpPost]
        public async Task<ActionResult<VentaDto>> Create([FromBody] VentaDto dto)
        {
            var venta = await _context.Ventas
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id_venta == id);

            if (venta == null) return NotFound();

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

                foreach (var d in dto.Detalles)
                {
                    var producto = await _context.Productos.FindAsync(d.Id_producto);
                    if (producto == null)
                    {
                        return BadRequest(new { mensaje = $"El producto con ID {d.Id_producto} no existe." });
                    }

                    if (producto.Stock_actual < d.Cantidad_o_gramos)
                    {
                        return BadRequest(new { mensaje = $"Stock insuficiente para el producto '{producto.Nombre}'. Disponible: {producto.Stock_actual}" });
                    }

                    // Descuento atómico de inventario
                    producto.Stock_actual -= d.Cantidad_o_gramos;

                    venta.Detalles.Add(new DetalleVenta
                    {
                        Id_producto = d.Id_producto,
                        Cantidad_o_gramos = d.Cantidad_o_gramos,
                        Precio_aplicado = d.Precio_aplicado,
                        Subtotal = d.Subtotal
                    });
                }

                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                dto.Id_venta = venta.Id_venta;
                return Ok(dto);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { mensaje = "Error interno al procesar la venta", detalle = ex.Message });
            }
        }
    }
}