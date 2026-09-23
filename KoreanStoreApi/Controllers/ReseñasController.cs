using KoreanStoreApi.Data;
using KoreanStoreApi.DTOs;
using KoreanStoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoreanStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReseñasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ReseñasController(ApplicationDbContext context) => _context = context;

        // GET: api/reseñas/producto/5
        [HttpGet("producto/{productoId}")]
        public async Task<ActionResult<IEnumerable<ReseñaDto>>> GetByProducto(int productoId)
        {
            var lista = await _context.Reseñas
                .Include(r => r.Usuario)
                .Where(r => r.Id_producto == productoId)
                .Select(r => new ReseñaDto
                {
                    Id = r.Id,
                    Id_user = r.Id_user,
                    Id_producto = r.Id_producto,
                    Calificacion = r.Calificacion,
                    Comentario = r.Comentario,
                    Fecha = r.Fecha,
                    NombreUsuario = r.Usuario != null ? r.Usuario.NombreUsuario : null
                })
                .ToListAsync();
            return Ok(lista);
        }

        // POST: api/reseñas
        [HttpPost]
        public async Task<ActionResult<ReseñaDto>> Create([FromBody] ReseñaDto dto)
        {
            var reseña = new Reseña
            {
                Id_user = dto.Id_user,
                Id_producto = dto.Id_producto,
                Calificacion = dto.Calificacion,
                Comentario = dto.Comentario,
                Fecha = DateTime.UtcNow
            };
            _context.Reseñas.Add(reseña);
            await _context.SaveChangesAsync();
            dto.Id = reseña.Id;
            return Ok(dto);
        }
    }
}