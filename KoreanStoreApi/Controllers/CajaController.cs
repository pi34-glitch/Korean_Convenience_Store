using KoreanStoreApi.Data;
using KoreanStoreApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoreanStoreApi.Controllers
{
    /// <summary>
    /// Endpoints específicos para la HU-04 (Jorge Mercado Calcina).
    /// Valida stock de productos por unidad antes de confirmar la venta.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CajaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CajaController(ApplicationDbContext context) => _context = context;

        /// <summary>
        /// POST: api/caja/validar-stock
        /// Verifica que la cantidad solicitada no supere el stock actual.
        /// </summary>
        [HttpPost("validar-stock")]
        public async Task<IActionResult> ValidarStock([FromBody] ValidarStockRequest request)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id_producto == request.ProductoId);

            if (producto == null)
                return NotFound(new { success = false, message = "Producto no encontrado." });

            if (request.Cantidad > producto.Stock_actual)
                return Ok(new
                {
                    success = false,
                    message = $"Stock insuficiente. Solo hay {producto.Stock_actual} unidades."
                });

            return Ok(new { success = true, message = "Stock validado correctamente." });
        }
    }

    /// <summary>
    /// DTO de petición para la validación de stock.
    /// </summary>
    public class ValidarStockRequest
    {
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
    }
}