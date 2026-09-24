using KoreanStoreApi.Data;
using KoreanStoreApi.DTOs;
using KoreanStoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoreanStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context) => _context = context;

        // GET: api/productos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetAll()
        {
            var lista = await _context.Productos
                .Include(p => p.Proveedor)
                .Select(p => new ProductoDto
                {
                    Id_producto = p.Id_producto,
                    Id_proveedor = p.Id_proveedor,
                    Nombre = p.Nombre,
                    Tipo_venta = p.Tipo_venta.ToString(),
                    Precio_unitario = p.Precio_unitario,
                    Stock_actual = p.Stock_actual,
                    Stock_minimo = p.Stock_minimo,
                    NombreProveedor = p.Proveedor != null ? p.Proveedor.Nombre : null
                })
                .ToListAsync();
            return Ok(lista);
        }

        // GET: api/productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetById(int id)
        {
            var p = await _context.Productos
                .Include(x => x.Proveedor)
                .FirstOrDefaultAsync(x => x.Id_producto == id);
            if (p == null) return NotFound();

            return Ok(new ProductoDto
            {
                Id_producto = p.Id_producto,
                Id_proveedor = p.Id_proveedor,
                Nombre = p.Nombre,
                Tipo_venta = p.Tipo_venta.ToString(),
                Precio_unitario = p.Precio_unitario,
                Stock_actual = p.Stock_actual,
                Stock_minimo = p.Stock_minimo,
                NombreProveedor = p.Proveedor?.Nombre
            });
        }

        // POST: api/productos
        [HttpPost]
        public async Task<ActionResult<ProductoDto>> Create([FromBody] ProductoDto dto)
        {
            if (!Enum.TryParse<TipoVenta>(dto.Tipo_venta, out var tipo))
                return BadRequest(new { mensaje = "Tipo_venta inválido" });

            var producto = new Producto
            {
                Id_proveedor = dto.Id_proveedor,
                Nombre = dto.Nombre,
                Tipo_venta = tipo,
                Precio_unitario = dto.Precio_unitario,
                Stock_actual = dto.Stock_actual,
                Stock_minimo = dto.Stock_minimo
            };
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            dto.Id_producto = producto.Id_producto;
            return CreatedAtAction(nameof(GetById), new { id = producto.Id_producto }, dto);
        }

        // PUT: api/productos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductoDto dto)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            if (!Enum.TryParse<TipoVenta>(dto.Tipo_venta, out var tipo))
                return BadRequest(new { mensaje = "Tipo_venta inválido" });

            producto.Id_proveedor = dto.Id_proveedor;
            producto.Nombre = dto.Nombre;
            producto.Tipo_venta = tipo;
            producto.Precio_unitario = dto.Precio_unitario;
            producto.Stock_actual = dto.Stock_actual;
            producto.Stock_minimo = dto.Stock_minimo;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PATCH: api/productos/5/tarifa
        // Permite actualizar únicamente el precio/tarifa de un producto específico
        [HttpPatch("{id}/tarifa")]
        public async Task<IActionResult> UpdateTarifa(int id, [FromBody] decimal nuevoPrecio)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound(new { mensaje = "Producto no encontrado." });

            producto.Precio_unitario = nuevoPrecio;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Tarifa/Precio actualizada exitosamente.", id_producto = producto.Id_producto, precio_nuevo = producto.Precio_unitario });
        }

        // GET: api/productos/alertas-stock
        // Consulta los productos cuyo stock actual está por debajo o igual al stock mínimo
        [HttpGet("alertas-stock")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetAlertasStock()
        {
            var alertas = await _context.Productos
                .Include(p => p.Proveedor)
                .Where(p => p.Stock_actual <= p.Stock_minimo)
                .Select(p => new ProductoDto
                {
                    Id_producto = p.Id_producto,
                    Id_proveedor = p.Id_proveedor,
                    Nombre = p.Nombre,
                    Tipo_venta = p.Tipo_venta.ToString(),
                    Precio_unitario = p.Precio_unitario,
                    Stock_actual = p.Stock_actual,
                    Stock_minimo = p.Stock_minimo,
                    NombreProveedor = p.Proveedor != null ? p.Proveedor.Nombre : null
                })
                .ToListAsync();

            return Ok(alertas);
        }
    }
}