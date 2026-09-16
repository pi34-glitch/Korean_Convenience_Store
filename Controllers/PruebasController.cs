using Microsoft.AspNetCore.Mvc;

namespace Korean_Convenience_Store.Controllers
{
    /// <summary>
    /// Módulo de verificación de la HU-04 (Sprint 1).
    /// Calcula el subtotal por producto según su precio unitario y cantidad.
    /// Accesible desde: /Pruebas
    /// </summary>
    public class PruebasController : Controller
    {
        public IActionResult Index()
        {
            // Simulamos un carrito con productos seleccionados
            // Cada línea representa: producto, precio unitario, cantidad
            var carrito = new List<CalculoProducto>
            {
                new CalculoProducto
                {
                    Producto = "Ramen Instantáneo",
                    PrecioUnitario = 25.00m,
                    Cantidad = 2
                },
                new CalculoProducto
                {
                    Producto = "Bebida Milkis 250ml",
                    PrecioUnitario = 6.00m,
                    Cantidad = 3
                },
                new CalculoProducto
                {
                    Producto = "Snack Pocky Chocolate",
                    PrecioUnitario = 5.50m,
                    Cantidad = 4
                },
                new CalculoProducto
                {
                    Producto = "Tteokbokki Picante",
                    PrecioUnitario = 12.00m,
                    Cantidad = 1
                }
            };

            return View(carrito);
        }
    }

    /// <summary>
    /// Representa una línea del carrito: producto, precio unitario y cantidad.
    /// Calcula automáticamente el subtotal.
    /// </summary>
    public class CalculoProducto
    {
        public string Producto { get; set; } = string.Empty;
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }

        /// <summary>
        /// Subtotal = Precio Unitario × Cantidad
        /// </summary>
        public decimal Subtotal => PrecioUnitario * Cantidad;
    }
}