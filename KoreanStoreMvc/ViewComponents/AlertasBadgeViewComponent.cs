using KoreanStoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace KoreanStoreMvc.ViewComponents
{
    public class AlertasBadgeViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AlertasBadgeViewComponent> _logger;

        public AlertasBadgeViewComponent(
            IHttpClientFactory httpClientFactory,
            ILogger<AlertasBadgeViewComponent> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

                var productos = await client.GetFromJsonAsync<List<ProductoModel>>(
                    "api/productos/alertas"
                );

                var cantidadAlertas = productos?.Count ?? 0;

                return View(cantidadAlertas);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error al consultar alertas para el badge"
                );

                return View(0);
            }
        }
    }
}