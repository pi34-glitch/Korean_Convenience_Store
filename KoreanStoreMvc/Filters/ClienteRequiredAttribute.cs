using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KoreanStoreMvc.Filters
{
    /// <summary>
    /// Filtro que requiere que el usuario tenga rol "Cliente".
    /// Si no está logueado o no es Cliente, redirige al Login o AccesoDenegado.
    /// </summary>
    public class ClienteRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = context.HttpContext.Session.GetInt32("UserId");
            var userRole = context.HttpContext.Session.GetString("UserRole");

            if (userId == null)
            {
                // No está logueado → redirigir al Login
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            if (userRole != "Cliente")
            {
                // Está logueado pero no es Cliente → Acceso Denegado
                context.Result = new RedirectToActionResult("AccesoDenegado", "Account", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}