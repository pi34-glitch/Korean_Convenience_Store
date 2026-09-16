using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Korean_Convenience_Store.Filters
{
    public class RolRequeridoAttribute : ActionFilterAttribute
    {
        private readonly string[] _rolesPermitidos;

        public RolRequeridoAttribute(params string[] rolesPermitidos)
        {
            _rolesPermitidos = rolesPermitidos;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var idUsuario = session.GetInt32("IdUsuario");
            var rol = session.GetString("Rol");

            if (idUsuario == null || string.IsNullOrEmpty(rol))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            if (_rolesPermitidos.Length > 0 && !_rolesPermitidos.Contains(rol))
            {
                context.Result = new RedirectToActionResult("AccesoDenegado", "Account", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}