using Microsoft.AspNetCore.Mvc;

namespace Korean_Convenience_Store.Controllers
{
    /// <summary>
    /// HU-04 - Módulo de verificación del cálculo por precio unitario (FASE 4)
    /// </summary>
    public class PruebasController : Controller
    {
        public IActionResult Index()
        {
            var carrito = new List<CalculoProducto>
            {
                new CalculoProducto { Producto = "Ramen Instantáneo", PrecioUnitario = 25.00m, Cantidad = 2 },
                new CalculoProducto { Producto = "Bebida Milkis 250ml", PrecioUnitario = 6.00m, Cantidad = 3 },
                new CalculoProducto { Producto = "Snack Pocky Chocolate", PrecioUnitario = 5.50m, Cantidad = 4 },
                new CalculoProducto { Producto = "Tteokbokki Picante", PrecioUnitario = 12.00m, Cantidad = 1 }
            };

            return View(carrito);
        }
    }

    public class CalculoProducto
    {
        public string Producto { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }

        public decimal Subtotal => PrecioUnitario * Cantidad;
    }
}