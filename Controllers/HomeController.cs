using Korean_Convenience_Store.Filters;
using Korean_Convenience_Store.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Korean_Convenience_Store.Controllers
{
    [RolRequerido] // ← sin roles = cualquier usuario logueado
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.NombreUsuario = HttpContext.Session.GetString("NombreUsuario");
            ViewBag.Rol = HttpContext.Session.GetString("Rol");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}