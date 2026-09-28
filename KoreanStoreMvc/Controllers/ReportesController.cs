using KoreanStoreMvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace KoreanStoreMvc.Controllers
{
    [SessionRequired]
    public class ReportesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ReportesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            try
            {
                var reporte = await client.GetFromJsonAsync<ReporteVentasModel>(
                    "api/reportes/ventas-diarias"
                );

                return View(reporte);
            }
            catch
            {
                return View(new ReporteVentasModel());
            }
        }
    }

    public class ReporteVentasModel
    {
        public string Fecha { get; set; } = string.Empty;
        public decimal TotalVentas { get; set; }
        public decimal Efectivo { get; set; }
        public decimal Qr { get; set; }
    }
}