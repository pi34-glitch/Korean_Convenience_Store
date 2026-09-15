using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Korean_Convenience_Store.Filters
{
    /// <summary>
    /// Filtro simple que verifica que haya una sesión activa con el rol indicado.
    /// Uso: [RolRequerido("Administrador")] o [RolRequerido("Administrador","Cajero")]
    /// Si no hay sesión → redirige a Login.
    /// Si hay sesión pero el rol no coincide → redirige a AccesoDenegado.
    /// </summary>
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

            // Sin sesión → al Login
            if (idUsuario == null || string.IsNullOrEmpty(rol))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Con sesión pero rol incorrecto → Acceso denegado
            if (_rolesPermitidos.Length > 0 && !_rolesPermitidos.Contains(rol))
            {
                context.Result = new RedirectToActionResult("AccesoDenegado", "Account", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}