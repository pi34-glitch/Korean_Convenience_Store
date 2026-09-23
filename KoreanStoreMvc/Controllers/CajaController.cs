using KoreanStoreMvc.Filters;
using KoreanStoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace KoreanStoreMvc.Controllers
{
    /// <summary>
    /// HU-04 - Selección de productos por unidad (Jorge Mercado Calcina).
    /// Este controlador MVC consume la API para obtener productos y validar stock.
    /// </summary>
    [SessionRequired]
    public class CajaController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CajaController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Caja/Index
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            try
            {
                var productos = await client.GetFromJsonAsync<List<ProductoModel>>("api/productos");
                return View(productos ?? new List<ProductoModel>());
            }
            catch
            {
                // Si falla la API, devolver lista vacía
                return View(new List<ProductoModel>());
            }
        }

        // POST: /Caja/ValidarStock
        [HttpPost]
        public async Task<IActionResult> ValidarStock([FromBody] ValidarStockRequest request)
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            try
            {
                var response = await client.PostAsJsonAsync("api/caja/validar-stock", request);
                var contenido = await response.Content.ReadAsStringAsync();
                return Content(contenido, "application/json");
            }
            catch
            {
                return Json(new { success = false, message = "Error al conectar con la API." });
            }
        }
    }

    public class ValidarStockRequest
    {
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
    }
}