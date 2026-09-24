using KoreanStoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace KoreanStoreMvc.Controllers
{
    public class CalculadoraController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CalculadoraController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Calculadora/Index
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            try
            {
                // Obtener productos desde la API
                var productos = await client
                    .GetFromJsonAsync<List<ProductoModel>>("api/productos");

                ViewBag.Productos = productos ?? new List<ProductoModel>();

                // Crear un carrito vacío para la vista
                return View(new CarritoModel());
            }
            catch
            {
                ViewBag.Productos = new List<ProductoModel>();

                return View(new CarritoModel());
            }
        }

        // POST: /Calculadora/Index
        [HttpPost]
        public async Task<IActionResult> Index(CarritoModel carrito)
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            try
            {
                // Obtener los productos desde la API
                var productos = await client
                    .GetFromJsonAsync<List<ProductoModel>>("api/productos");

                productos ??= new List<ProductoModel>();

                ViewBag.Productos = productos;

                // Buscar el producto seleccionado
                var producto = productos
                    .FirstOrDefault(p => p.Id_producto == carrito.Id_producto);

                if (producto == null)
                {
                    ModelState.AddModelError(
                        "",
                        "El producto seleccionado no existe."
                    );

                    return View(carrito);
                }

                // Validar cantidad
                if (carrito.CantidadGramo <= 0)
                {
                    ModelState.AddModelError(
                        "",
                        "La cantidad de gramos debe ser mayor que 0."
                    );

                    return View(carrito);
                }

                // Calcular subtotal
                carrito.Subtotal =
                    producto.Precio_unitario * carrito.CantidadGramo;

                // Guardar nombre para mostrarlo en la vista
                carrito.NombreProducto = producto.Nombre;

                return View(carrito);
            }
            catch
            {
                ViewBag.Productos = new List<ProductoModel>();

                ModelState.AddModelError(
                    "",
                    "No se pudo conectar con la API."
                );

                return View(carrito);
            }
        }
    }
}