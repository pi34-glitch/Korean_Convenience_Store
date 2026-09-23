using KoreanStoreApi.Data;
using KoreanStoreApi.DTOs;
using KoreanStoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoreanStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarritosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CarritosController(ApplicationDbContext context) => _context = context;

        // GET: api/carritos/usuario/5
        [HttpGet("usuario/{userId}")]
        public async Task<ActionResult<IEnumerable<CarritoDto>>> GetByUsuario(int userId)
        {
            var items = await _context.Carritos
                .Include(c => c.Producto)
                .Where(c => c.Id_user == userId)
                .Select(c => new CarritoDto
                {
                    Id_carrito = c.Id_carrito,
                    Id_user = c.Id_user,
                    Id_producto = c.Id_producto,
                    CantidadUnitario = c.CantidadUnitario,
                    CantidadGramo = c.CantidadGramo,
                    Subtotal = c.Subtotal,
                    NombreProducto = c.Producto != null ? c.Producto.Nombre : null
                })
                .ToListAsync();
            return Ok(items);
        }

        // POST: api/carritos
        [HttpPost]
        public async Task<ActionResult<CarritoDto>> Add([FromBody] CarritoDto dto)
        {
            var carrito = new Carrito
            {
                Id_user = dto.Id_user,
                Id_producto = dto.Id_producto,
                CantidadUnitario = dto.CantidadUnitario,
                CantidadGramo = dto.CantidadGramo,
                Subtotal = dto.Subtotal
            };
            _context.Carritos.Add(carrito);
            await _context.SaveChangesAsync();
            dto.Id_carrito = carrito.Id_carrito;
            return Ok(dto);
        }

        // DELETE: api/carritos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Carritos.FindAsync(id);
            if (item == null) return NotFound();
            _context.Carritos.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/carritos/usuario/5/vaciar
        [HttpDelete("usuario/{userId}/vaciar")]
        public async Task<IActionResult> Vaciar(int userId)
        {
            var items = await _context.Carritos.Where(c => c.Id_user == userId).ToListAsync();
            _context.Carritos.RemoveRange(items);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}