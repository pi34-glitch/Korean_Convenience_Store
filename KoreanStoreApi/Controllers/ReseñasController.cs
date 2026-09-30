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

        // =========================================================
        // GET: api/reseñas
        // (Opcional) Listar todas las reseñas
        // =========================================================
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReseñaDto>>> GetAll()
        {
            var lista = await _context.Reseñas
                .Include(r => r.Usuario)
                .OrderByDescending(r => r.Fecha)
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

        // =========================================================
        // GET: api/reseñas/producto/5
        // Listar reseñas de un producto específico
        // =========================================================
        [HttpGet("producto/{productoId}")]
        public async Task<ActionResult<IEnumerable<ReseñaDto>>> GetByProducto(int productoId)
        {
            var lista = await _context.Reseñas
                .Include(r => r.Usuario)
                .Where(r => r.Id_producto == productoId)
                .OrderByDescending(r => r.Fecha)
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

        // =========================================================
        // GET: api/reseñas/5
        // Obtener una reseña por ID (para Edit/Delete en el MVC)
        // =========================================================
        [HttpGet("{id}")]
        public async Task<ActionResult<ReseñaDto>> GetById(int id)
        {
            var reseña = await _context.Reseñas
                .Include(r => r.Usuario)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reseña == null)
                return NotFound(new { mensaje = "La reseña no existe." });

            var dto = new ReseñaDto
            {
                Id = reseña.Id,
                Id_user = reseña.Id_user,
                Id_producto = reseña.Id_producto,
                Calificacion = reseña.Calificacion,
                Comentario = reseña.Comentario,
                Fecha = reseña.Fecha,
                NombreUsuario = reseña.Usuario?.NombreUsuario
            };

            return Ok(dto);
        }

        // =========================================================
        // POST: api/reseñas
        // Crear una nueva reseña
        // =========================================================
        [HttpPost]
        public async Task<ActionResult<ReseñaDto>> Create([FromBody] ReseñaDto dto)
        {
            // Validación básica de calificación
            if (dto.Calificacion < 1 || dto.Calificacion > 5)
                return BadRequest(new { mensaje = "La calificación debe estar entre 1 y 5." });

            // Validar que el usuario exista
            var usuarioExiste = await _context.Usuarios.AnyAsync(u => u.Id == dto.Id_user);
            if (!usuarioExiste)
                return BadRequest(new { mensaje = "El usuario no existe." });

            // Validar que el producto exista
            var productoExiste = await _context.Productos.AnyAsync(p => p.Id_producto == dto.Id_producto);
            if (!productoExiste)
                return BadRequest(new { mensaje = "El producto no existe." });

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
            dto.Fecha = reseña.Fecha;

            return CreatedAtAction(nameof(GetById), new { id = reseña.Id }, dto);
        }

        // =========================================================
        // PUT: api/reseñas/5
        // Actualizar una reseña existente (para Edit en el MVC)
        // =========================================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ReseñaDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { mensaje = "El ID de la reseña no coincide." });

            if (dto.Calificacion < 1 || dto.Calificacion > 5)
                return BadRequest(new { mensaje = "La calificación debe estar entre 1 y 5." });

            var reseña = await _context.Reseñas.FindAsync(id);
            if (reseña == null)
                return NotFound(new { mensaje = "La reseña no existe." });

            // (Opcional) Validar que solo el dueño pueda editar
            if (reseña.Id_user != dto.Id_user)
                return Forbid();

            reseña.Calificacion = dto.Calificacion;
            reseña.Comentario = dto.Comentario;
            reseña.Fecha = DateTime.UtcNow; // Actualizamos la fecha al editar

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Reseñas.AnyAsync(r => r.Id == id))
                    return NotFound(new { mensaje = "La reseña no existe." });
                throw;
            }

            return Ok(new { mensaje = "Reseña actualizada correctamente." });
        }

        // =========================================================
        // DELETE: api/reseñas/5
        // Eliminar una reseña (para Delete en el MVC)
        // =========================================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var reseña = await _context.Reseñas.FindAsync(id);
            if (reseña == null)
                return NotFound(new { mensaje = "La reseña no existe." });

            _context.Reseñas.Remove(reseña);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Reseña eliminada correctamente." });
        }

        // =========================================================
        // GET: api/reseñas/promedio/5
        // (Opcional) Promedio de calificación de un producto
        // =========================================================
        [HttpGet("promedio/{productoId}")]
        public async Task<ActionResult> GetPromedio(int productoId)
        {
            var resenias = await _context.Reseñas
                .Where(r => r.Id_producto == productoId)
                .ToListAsync();

            if (!resenias.Any())
                return Ok(new { productoId, promedio = 0.0, total = 0 });

            var promedio = resenias.Average(r => r.Calificacion);

            return Ok(new
            {
                productoId,
                promedio = Math.Round(promedio, 2),
                total = resenias.Count
            });
        }
    }
}