using KoreanStoreApi.Data;
using KoreanStoreApi.DTOs;
using KoreanStoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoreanStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProveedoresController(ApplicationDbContext context) => _context = context;

        // GET: api/proveedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedorDto>>> GetAll()
        {
            var lista = await _context.Proveedores
                .Select(p => new ProveedorDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Contacto = p.Contacto,
                    Telefono = p.Telefono,
                    Direccion = p.Direccion
                })
                .ToListAsync();
            return Ok(lista);
        }

        // GET: api/proveedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorDto>> GetById(int id)
        {
            var p = await _context.Proveedores.FindAsync(id);
            if (p == null) return NotFound();

            return Ok(new ProveedorDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Contacto = p.Contacto,
                Telefono = p.Telefono,
                Direccion = p.Direccion
            });
        }

        // POST: api/proveedores
        [HttpPost]
        public async Task<ActionResult<ProveedorDto>> Create([FromBody] ProveedorDto dto)
        {
            var proveedor = new Proveedor
            {
                Nombre = dto.Nombre,
                Contacto = dto.Contacto,
                Telefono = dto.Telefono,
                Direccion = dto.Direccion
            };
            _context.Proveedores.Add(proveedor);
            await _context.SaveChangesAsync();

            dto.Id = proveedor.Id;
            return CreatedAtAction(nameof(GetById), new { id = proveedor.Id }, dto);
        }

        // PUT: api/proveedores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProveedorDto dto)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return NotFound();

            proveedor.Nombre = dto.Nombre;
            proveedor.Contacto = dto.Contacto;
            proveedor.Telefono = dto.Telefono;
            proveedor.Direccion = dto.Direccion;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/proveedores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null) return NotFound();

            _context.Proveedores.Remove(proveedor);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}