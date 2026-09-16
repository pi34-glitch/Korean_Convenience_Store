using Korean_Convenience_Store.Data;
using Korean_Convenience_Store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Korean_Convenience_Store.Controllers
{
    public class CajaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CajaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos
                .Where(p => p.StockActual > 0)
                .OrderBy(p => p.Categoria)
                .ThenBy(p => p.Nombre)
                .ToListAsync();

            return View(productos);
        }

        [HttpPost]
        public async Task<IActionResult> ValidarStock([FromBody] ValidarStockRequest request)
        {
            var producto = await _context.Productos.FindAsync(request.ProductoId);

            if (producto == null)
                return Json(new { success = false, message = "Producto no encontrado." });

            if (request.Cantidad > producto.StockActual)
                return Json(new { success = false, message = $"Stock insuficiente. Solo hay {producto.StockActual} unidades disponibles." });

            return Json(new { success = true, message = "Stock validado correctamente." });
        }
    }

    public class ValidarStockRequest
    {
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
    }
}