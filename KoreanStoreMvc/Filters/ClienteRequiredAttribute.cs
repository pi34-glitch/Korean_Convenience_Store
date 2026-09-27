using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KoreanStoreMvc.Filters
{
    /// <summary>
    /// Sprint 3 - HU-10 (Jorge Mercado Calcina).
    /// Permite el acceso solo a usuarios con sesión activa y rol "Cliente".
    /// </summary>
    public class ClienteRequiredAttribute : ActionFilterAttribute
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

            // 2. Con sesión pero sin rol Cliente → acceso denegado
            if (string.IsNullOrEmpty(userRole) || userRole != "Cliente")
            {
                context.Result = new RedirectToActionResult("AccesoDenegado", "Account", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}