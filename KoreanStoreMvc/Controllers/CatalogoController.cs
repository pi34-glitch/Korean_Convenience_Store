using KoreanStoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace KoreanStoreMvc.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CatalogoController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Catalogo
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            try
            {
                var productos = await client
                    .GetFromJsonAsync<List<ProductoModel>>("api/productos");

                return View(productos ?? new List<ProductoModel>());
            }
            catch
            {
                return View(new List<ProductoModel>());
            }
        }
    }
}