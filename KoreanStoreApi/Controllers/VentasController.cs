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

        // GET: api/ventas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VentaDto>> GetById(int id)
        {
            var venta = await _context.Ventas
                .Include(v => v.Detalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id_venta == id);

            if (venta == null) return NotFound();

            return Ok(new VentaDto
            {
                Id_venta = venta.Id_venta,
                Id_user = venta.Id_user,
                Fecha_venta = venta.Fecha_venta,
                Total = venta.Total,
                Metodo_pago = venta.Metodo_pago.ToString(),
                Detalles = venta.Detalles.Select(d => new DetalleVentaDto
                {
                    Id_detalle = d.Id_detalle,
                    Id_venta = d.Id_venta,
                    Id_producto = d.Id_producto,
                    Cantidad_o_gramos = d.Cantidad_o_gramos,
                    Precio_aplicado = d.Precio_aplicado,
                    Subtotal = d.Subtotal,
                    NombreProducto = d.Producto?.Nombre
                }).ToList()
            });
        }

        // POST: api/ventas  → Cobro transaccional con descuento de stock
        [HttpPost]
        public async Task<ActionResult<VentaDto>> Create([FromBody] VentaDto dto)
        {
            // 1. Validaciones iniciales
            if (dto == null || dto.Detalles == null || !dto.Detalles.Any())
                return BadRequest(new { mensaje = "El ticket no contiene productos." });

            if (!Enum.TryParse<MetodoPago>(dto.Metodo_pago, out var metodo))
                return BadRequest(new { mensaje = "Método de pago inválido. Use: Efectivo, QR o Tarjeta." });

            // 2. Abrir transacción atómica
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // 3. Crear la cabecera de la venta
                var venta = new Venta
                {
                    Id_user = dto.Id_user,
                    Fecha_venta = DateTime.UtcNow,
                    Total = dto.Total,
                    Metodo_pago = metodo
                };
                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();

                // 4. Procesar cada detalle: validar stock y descontar
                foreach (var item in dto.Detalles)
                {
                    var producto = await _context.Productos
                        .FirstOrDefaultAsync(p => p.Id_producto == item.Id_producto);

                    if (producto == null)
                        throw new Exception($"El producto con ID {item.Id_producto} no existe.");

                    // Validar stock suficiente (aplica para Unidad y PesoGramos)
                    if (producto.Stock_actual < item.Cantidad_o_gramos)
                        throw new Exception($"Stock insuficiente para \"{producto.Nombre}\". Disponible: {producto.Stock_actual}, solicitado: {item.Cantidad_o_gramos}.");

                    // Descuento atómico de existencias
                    producto.Stock_actual -= item.Cantidad_o_gramos;

                    // Crear el detalle
                    var detalle = new DetalleVenta
                    {
                        Id_venta = venta.Id_venta,
                        Id_producto = item.Id_producto,
                        Cantidad_o_gramos = item.Cantidad_o_gramos,
                        Precio_aplicado = producto.Precio_unitario,
                        Subtotal = item.Cantidad_o_gramos * producto.Precio_unitario
                    };
                    _context.DetallesVenta.Add(detalle);
                }

                // 5. Guardar todos los cambios
                await _context.SaveChangesAsync();

                // 6. Confirmar transacción
                await transaction.CommitAsync();

                dto.Id_venta = venta.Id_venta;
                dto.Fecha_venta = venta.Fecha_venta;

                return Ok(new
                {
                    exito = true,
                    idVenta = venta.Id_venta,
                    mensaje = "Venta procesada con éxito.",
                    venta = dto
                });
            }
            catch (Exception ex)
            {
                // 7. Revertir todo si algo falla
                await transaction.RollbackAsync();
                return StatusCode(500, new { exito = false, mensaje = "Error al procesar el cobro: " + ex.Message });
            }
        }
    }
}