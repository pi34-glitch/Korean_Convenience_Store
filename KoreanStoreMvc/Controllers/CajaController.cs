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

        // POST: /Caja/Cobrar
        [HttpPost]
        public async Task<IActionResult> Cobrar([FromBody] CobrarRequest request)
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Unauthorized(new { exito = false, mensaje = "Sesión expirada." });

            var ventaDto = new
            {
                id_user = userId.Value,
                total = request.Total,
                metodo_pago = request.MetodoPago,
                detalles = request.Detalles.Select(d => new
                {
                    id_producto = d.Id_producto,
                    cantidad_o_gramos = d.Cantidad,
                    precio_aplicado = d.Precio,
                    subtotal = d.Cantidad * d.Precio
                }).ToList()
            };

            try
            {
                var response = await client.PostAsJsonAsync("api/ventas", ventaDto);
                var contenido = await response.Content.ReadAsStringAsync();
                return Content(contenido, "application/json");
            }
            catch (Exception ex)
            {
                return Json(new { exito = false, mensaje = "Error al conectar con la API: " + ex.Message });
            }
        }

        // GET: /Caja/Ticket/5
        public async Task<IActionResult> Ticket(int id)
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            try
            {
                var venta = await client.GetFromJsonAsync<VentaModel>($"api/ventas/{id}");
                if (venta == null) return NotFound();
                return View(venta);
            }
            catch
            {
                return NotFound();
            }
        }
    }

    public class ValidarStockRequest
    {
        public int ProductoId { get; set; }
        public decimal Cantidad { get; set; }
    }
    public class CobrarRequest
    {
        public decimal Total { get; set; }
        public string MetodoPago { get; set; } = "Efectivo";
        public List<CobrarItem> Detalles { get; set; } = new();
    }

    public class CobrarItem
    {
        public int Id_producto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
    }
}