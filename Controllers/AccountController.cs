using Korean_Convenience_Store.Data;
using Korean_Convenience_Store.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            // Si ya está autenticado, redirigir al home
            if (User.Identity != null && User.Identity.IsAuthenticated)
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

            // Rehash automático si el algoritmo cambió
            if (resultado == PasswordVerificationResult.SuccessRehashNeeded)
            {
                usuario.PasswordHash = _hasher.HashPassword(usuario, password);
                await _context.SaveChangesAsync();
            }

            // Construir los Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol.ToString())
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = false,
                    ExpiresUtc = DateTime.UtcNow.AddHours(2)
                });

            // Redirigir según rol (por ahora ambos van a Home/Index)
            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}