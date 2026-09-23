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
        [HttpPost]
        public async Task<ActionResult<VentaDto>> Create([FromBody] VentaDto dto)
        {
            if (!Enum.TryParse<MetodoPago>(dto.Metodo_pago, out var metodo))
                return BadRequest(new { mensaje = "Método de pago inválido" });

            var venta = new Venta
            {
                Id_user = dto.Id_user,
                Fecha_venta = DateTime.UtcNow,
                Total = dto.Total,
                Metodo_pago = metodo,
                Detalles = dto.Detalles.Select(d => new DetalleVenta
                {
                    Id_producto = d.Id_producto,
                    Cantidad_o_gramos = d.Cantidad_o_gramos,
                    Precio_aplicado = d.Precio_aplicado,
                    Subtotal = d.Subtotal
                }).ToList()
            };
            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync();

            dto.Id_venta = venta.Id_venta;
            return Ok(dto);
        }
    }
}