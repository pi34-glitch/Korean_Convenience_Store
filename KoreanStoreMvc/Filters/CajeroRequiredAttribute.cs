using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KoreanStoreMvc.Filters
{
    /// <summary>
    /// HU-04 (Jorge Mercado Calcina).
    /// Permite el acceso solo a usuarios con sesión activa y rol "CajeroVendedor".
    /// Si no hay sesión → redirige al Login.
    /// Si hay sesión pero el rol no es CajeroVendedor → redirige a AccesoDenegado.
    /// </summary>
    public class CajeroRequiredAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var userId = httpContext.Session.GetInt32("UserId");
            var userRole = httpContext.Session.GetString("UserRole");

            // 1. Si no hay sesión, al login
            if (userId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // 2. Si hay sesión pero el rol no es CajeroVendedor, denegar acceso
            if (string.IsNullOrEmpty(userRole) || userRole != "CajeroVendedor")
            {
                context.Result = new RedirectToActionResult("AccesoDenegado", "Account", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}