using Microsoft.AspNetCore.Mvc;
using Korean_Convenience_Store.Filters;

namespace Korean_Convenience_Store.Controllers
{
    [RolRequerido("Cajero", "Administrador")]
    public class CajeroController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}