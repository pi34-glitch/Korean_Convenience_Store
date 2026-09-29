using KoreanStoreMvc.Filters;
using KoreanStoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace KoreanStoreMvc.Controllers
{
    /// <summary>
    /// Sprint 3 - HU-10 (Jorge Mercado Calcina).
    /// Módulo de Reseñas y Valoraciones de Comensales.
    /// Solo accesible para usuarios con rol "Cliente".
    /// </summary>
    [ClienteRequired]
    public class ReseniasController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ReseniasController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Reseñas?productoId=5
        public async Task<IActionResult> Index(int? productoId)
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            List<ReseñaModel> resenias = new();

            if (productoId.HasValue)
            {
                try
                {
                    resenias = await client.GetFromJsonAsync<List<ReseñaModel>>($"api/reseñas/producto/{productoId}")
                              ?? new List<ReseñaModel>();

                    var producto = await client.GetFromJsonAsync<ProductoModel>($"api/productos/{productoId}");
                    ViewBag.Producto = producto;
                }
                catch
                {
                    ViewBag.Error = "No se pudieron cargar las reseñas.";
                }
            }

            ViewBag.ProductoId = productoId;
            return View(resenias);
        }

        // GET: /Reseñas/Crear/5
        public async Task<IActionResult> Crear(int productoId)
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            try
            {
                var producto = await client.GetFromJsonAsync<ProductoModel>($"api/productos/{productoId}");
                if (producto == null) return NotFound();

                ViewBag.Producto = producto;
                return View(new ReseñaModel { Id_producto = productoId });
            }
            catch
            {
                return NotFound();
            }
        }

        // POST: /Reseñas/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ReseñaModel model)
        {
            // Obtener el ID del usuario logueado
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            // Validar calificación
            if (model.Calificacion < 1 || model.Calificacion > 5)
                ModelState.AddModelError("Calificacion", "La calificación debe estar entre 1 y 5 estrellas.");

            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            if (!ModelState.IsValid)
            {
                try
                {
                    ViewBag.Producto = await client.GetFromJsonAsync<ProductoModel>($"api/productos/{model.Id_producto}");
                }
                catch { }
                return View(model);
            }

            var dto = new
            {
                id = 0,
                id_user = userId.Value,
                id_producto = model.Id_producto,
                calificacion = model.Calificacion,
                comentario = model.Comentario ?? string.Empty,
                fecha = DateTime.UtcNow
            };

            try
            {
                var response = await client.PostAsJsonAsync("api/reseñas", dto);

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "No se pudo guardar la reseña.";
                    ViewBag.Producto = await client.GetFromJsonAsync<ProductoModel>($"api/productos/{model.Id_producto}");
                    return View(model);
                }

                TempData["Exito"] = "¡Gracias por tu reseña! Tu opinión ha sido registrada.";
                return RedirectToAction("Index", new { productoId = model.Id_producto });
            }
            catch
            {
                ViewBag.Error = "Error al conectar con la API.";
                return View(model);
            }
        }
    }
}