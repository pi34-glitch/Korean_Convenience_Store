using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KoreanStoreMvc.Filters
{
    /// <summary>
    /// Sprint 2 - HU-06 (Jorge Mercado Calcina).
    /// Permite el acceso solo a usuarios con sesión activa y rol "Administrador".
    /// Si no hay sesión → redirige al Login.
    /// Si hay sesión pero el rol no es Administrador → redirige a AccesoDenegado.
    /// </summary>
    public class AdministradorRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var userId = httpContext.Session.GetInt32("UserId");
            var userRole = httpContext.Session.GetString("UserRole");

            // 1. Sin sesión → al login
            if (userId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // 2. Con sesión pero sin rol Administrador → acceso denegado
            if (string.IsNullOrEmpty(userRole) || userRole != "Administrador")
            {
                context.Result = new RedirectToActionResult("AccesoDenegado", "Account", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}