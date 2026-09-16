using Korean_Convenience_Store.Data;
using Korean_Convenience_Store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Korean_Convenience_Store.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<Usuario> _hasher;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
            _hasher = new PasswordHasher<Usuario>();
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            // Si ya hay sesión, ir al Home
            if (HttpContext.Session.GetInt32("IdUsuario") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Debe ingresar correo y contraseña";
                return View();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email && u.Activo);

            if (usuario == null)
            {
                ViewBag.Error = "Credenciales incorrectas";
                return View();
            }

            var resultado = _hasher.VerifyHashedPassword(usuario, usuario.PasswordHash, password);
            if (resultado == PasswordVerificationResult.Failed)
            {
                ViewBag.Error = "Credenciales incorrectas";
                return View();
            }

            // ===== Guardar datos en la SESIÓN =====
            HttpContext.Session.SetInt32("IdUsuario", usuario.Id);
            HttpContext.Session.SetString("NombreUsuario", usuario.NombreUsuario);
            HttpContext.Session.SetString("Email", usuario.Email);
            HttpContext.Session.SetString("Rol", usuario.Rol.ToString());

            // ===== Redirección por ROL (HU-02) =====
            return usuario.Rol switch
            {
                RolUsuario.Cajero => RedirectToAction("Index", "Cajero"),
                RolUsuario.Administrador => RedirectToAction("Index", "Home"),
                _ => RedirectToAction("Index", "Home")
            };
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }

        // GET: /Account/AccesoDenegado
        [HttpGet]
        public IActionResult AccesoDenegado()
        {
            return View();
        }
    }
}