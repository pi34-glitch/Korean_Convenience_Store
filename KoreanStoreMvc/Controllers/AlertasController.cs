using KoreanStoreMvc.Filters;
using KoreanStoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace KoreanStoreMvc.Controllers
{
    /// <summary>
    /// HU-09 - Alertas Visuales de Stock Mínimo (Nils Oliver Machaca Mamani).
    /// Consume GET /api/productos/alertas y muestra los productos en estado crítico.
    /// Solo el Administrador puede acceder.
    /// </summary>
    [SessionRequired]
    public class AlertasController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AlertasController> _logger;

        public AlertasController(IHttpClientFactory httpClientFactory, ILogger<AlertasController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // GET: /Alertas/Index
        public async Task<IActionResult> Index()
        {
            // Verificar que sea Administrador
            var rol = HttpContext.Session.GetString("UserRole");
            if (rol != "Administrador")
            {
                return RedirectToAction("AccesoDenegado", "Account");
            }

            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            try
            {
                _logger.LogInformation("Consultando alertas de stock a la API: {BaseUrl}api/productos/alertas",
                    client.BaseAddress);

                var productos = await client.GetFromJsonAsync<List<ProductoModel>>("api/productos/alertas");

                if (productos == null)
                {
                    productos = new List<ProductoModel>();
                }

                _logger.LogInformation("Alertas cargadas: {Count}", productos.Count);
                return View(productos);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error HTTP al consultar alertas");
                ViewBag.Error = $"No se pudo conectar con la API. Verifica que esté corriendo en {client.BaseAddress}";
                return View(new List<ProductoModel>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al consultar alertas");
                ViewBag.Error = $"Error inesperado: {ex.Message}";
                return View(new List<ProductoModel>());
            }
        }
    }
}