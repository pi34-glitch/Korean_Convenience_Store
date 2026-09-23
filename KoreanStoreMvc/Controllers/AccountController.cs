using KoreanStoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace KoreanStoreMvc.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            // Si ya hay sesión, redirigir al home
            if (HttpContext.Session.GetInt32("UserId") != null)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");
            var response = await client.PostAsJsonAsync("api/usuarios/login", new
            {
                email = model.Email,
                password = model.Password
            });

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Credenciales incorrectas";
                return View(model);
            }

            var usuario = await response.Content.ReadFromJsonAsync<UsuarioModel>();
            if (usuario == null)
            {
                ViewBag.Error = "Error al procesar la respuesta";
                return View(model);
            }

            // Guardar datos en la sesión
            HttpContext.Session.SetInt32("UserId", usuario.Id);
            HttpContext.Session.SetString("UserName", usuario.NombreUsuario);
            HttpContext.Session.SetString("UserEmail", usuario.Email);
            HttpContext.Session.SetString("UserRole", usuario.Rol);

            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}