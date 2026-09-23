using KoreanStoreApi.Data;
using KoreanStoreApi.DTOs;
using KoreanStoreApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoreanStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<Usuario> _hasher = new();

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios
                .Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    NombreUsuario = u.NombreUsuario,
                    Email = u.Email,
                    Rol = u.Rol.ToString(),
                    Activo = u.Activo
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            return Ok(new UsuarioDto
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString(),
                Activo = usuario.Activo
            });
        }

        // POST: api/usuarios/registrar
        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioDto>> Registrar([FromBody] UsuarioCreateDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { mensaje = "El email ya está registrado" });

            if (!Enum.TryParse<RolUsuario>(dto.Rol, out var rol))
                return BadRequest(new { mensaje = "Rol inválido" });

            var usuario = new Usuario
            {
                NombreUsuario = dto.NombreUsuario,
                Email = dto.Email,
                Rol = rol,
                Activo = true
            };
            usuario.PasswordHash = _hasher.HashPassword(usuario, dto.Password);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return Ok(new UsuarioDto
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString(),
                Activo = usuario.Activo
            });
        }

        // POST: api/usuarios/login  (LOGIN SIMPLIFICADO SIN COOKIES NI CLAIMS)
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDto>> Login([FromBody] UsuarioLoginDto dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Activo);

            if (usuario == null)
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });

            var resultado = _hasher.VerifyHashedPassword(usuario, usuario.PasswordHash, dto.Password);

            if (resultado == PasswordVerificationResult.Failed)
                return Unauthorized(new { mensaje = "Credenciales incorrectas" });

            return Ok(new UsuarioDto
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                Rol = usuario.Rol.ToString(),
                Activo = usuario.Activo
            });
        }

        // DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null) return NotFound();

            usuario.Activo = false;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}