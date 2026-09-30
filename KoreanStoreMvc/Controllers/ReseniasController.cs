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
        private readonly ILogger<ReseniasController> _logger;

        public ReseniasController(IHttpClientFactory httpClientFactory, ILogger<ReseniasController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // GET: /Resenias
        // Listado de todos los productos con su promedio de calificación
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");
            var productos = new List<ProductoModel>();

            try
            {
                productos = await client.GetFromJsonAsync<List<ProductoModel>>("api/productos")
                            ?? new List<ProductoModel>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar productos para reseñas");
                ViewBag.Error = "No se pudieron cargar los productos.";
            }

            return View(productos);
        }

        // GET: /Resenias/PorProducto/5
        // Reseñas de un producto específico
        public async Task<IActionResult> PorProducto(int id)
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");
            var resenias = new List<ReseñaModel>();

            try
            {
                resenias = await client.GetFromJsonAsync<List<ReseñaModel>>($"api/reseñas/producto/{id}")
                          ?? new List<ReseñaModel>();

                var producto = await client.GetFromJsonAsync<ProductoModel>($"api/productos/{id}");
                ViewBag.Producto = producto;
                ViewBag.ProductoId = id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar reseñas del producto {Id}", id);
                ViewBag.Error = "No se pudieron cargar las reseñas.";
            }

            return View(resenias);
        }

        // GET: /Resenias/Create/5
        public async Task<IActionResult> Create(int productoId)
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

        // POST: /Resenias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReseñaModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

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
                return RedirectToAction("PorProducto", new { id = model.Id_producto });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear reseña");
                ViewBag.Error = "Error al conectar con la API.";
                return View(model);
            }
        }

        // GET: /Resenias/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            try
            {
                var resenia = await client.GetFromJsonAsync<ReseñaModel>($"api/reseñas/{id}");
                if (resenia == null) return NotFound();

                var producto = await client.GetFromJsonAsync<ProductoModel>($"api/productos/{resenia.Id_producto}");
                ViewBag.Producto = producto;

                return View(resenia);
            }
            catch
            {
                return NotFound();
            }
        }

        // POST: /Resenias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReseñaModel model)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return RedirectToAction("Login", "Account");

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
                id = id,
                id_user = userId.Value,
                id_producto = model.Id_producto,
                calificacion = model.Calificacion,
                comentario = model.Comentario ?? string.Empty,
                fecha = DateTime.UtcNow
            };

            try
            {
                var response = await client.PutAsJsonAsync($"api/reseñas/{id}", dto);

                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.Error = "No se pudo actualizar la reseña.";
                    ViewBag.Producto = await client.GetFromJsonAsync<ProductoModel>($"api/productos/{model.Id_producto}");
                    return View(model);
                }

                TempData["Exito"] = "Reseña actualizada correctamente.";
                return RedirectToAction("PorProducto", new { id = model.Id_producto });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar reseña");
                ViewBag.Error = "Error al conectar con la API.";
                return View(model);
            }
        }

        // GET: /Resenias/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            try
            {
                var resenia = await client.GetFromJsonAsync<ReseñaModel>($"api/reseñas/{id}");
                if (resenia == null) return NotFound();

                var producto = await client.GetFromJsonAsync<ProductoModel>($"api/productos/{resenia.Id_producto}");
                ViewBag.Producto = producto;

                return View(resenia);
            }
            catch
            {
                return NotFound();
            }
        }

        // POST: /Resenias/DeleteConfirmed/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int productoId)
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            try
            {
                var response = await client.DeleteAsync($"api/reseñas/{id}");

                if (response.IsSuccessStatusCode)
                    TempData["Exito"] = "Reseña eliminada correctamente.";
                else
                    TempData["Error"] = "No se pudo eliminar la reseña.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar reseña {Id}", id);
                TempData["Error"] = "Error al conectar con la API.";
            }

            return RedirectToAction("PorProducto", new { id = productoId });
        }
    }
}