using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using KoreanStoreMvc.Models;
using KoreanStoreMvc.Filters;

namespace KoreanStoreMvc.Controllers;

[SessionRequired]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    // Vistas del prototipo del Sprint 2 (se mantienen por compatibilidad)
    public IActionResult Tienda()
    {
        return View();
    }

    public IActionResult Tarifas()
    {
        return View();
    }

    // Redirige al nuevo controlador de Alertas (Sprint 3 - HU-09)
    public IActionResult AlertasStock()
    {
        return RedirectToAction("Index", "Alertas");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}