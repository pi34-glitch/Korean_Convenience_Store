using KoreanStoreMvc.Filters;
using KoreanStoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Json;

namespace KoreanStoreMvc.Controllers
{
    /// <summary>
    /// Sprint 2 - HU-06 (Jorge Mercado Calcina).
    /// CRUD de Productos desde el MVC consumiendo la API REST /api/productos.
    /// Solo accesible para usuarios con rol "Administrador".
    /// </summary>
    [AdministradorRequired]
    public class ProductosController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductosController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // ========================================================
        // GET: /Productos?tipoVenta=Unidad&buscar=Ramen
        // ========================================================
        public async Task<IActionResult> Index(string? tipoVenta, string? buscar)
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            List<ProductoModel> productos;
            try
            {
                productos = await client.GetFromJsonAsync<List<ProductoModel>>("api/productos")
                            ?? new List<ProductoModel>();
            }
            catch
            {
                productos = new List<ProductoModel>();
                ViewBag.Error = "No se pudo conectar con la API.";
            }

            // Filtro por tipo de venta
            if (!string.IsNullOrEmpty(tipoVenta))
                productos = productos.Where(p => p.Tipo_venta == tipoVenta).ToList();

            // Filtro por nombre
            if (!string.IsNullOrEmpty(buscar))
                productos = productos
                    .Where(p => p.Nombre.Contains(buscar, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            ViewBag.TipoVentaActual = tipoVenta;
            ViewBag.Buscar = buscar;

            return View(productos);
        }

        // ========================================================
        // GET: /Productos/Details/5
        // ========================================================
        public async Task<IActionResult> Details(int id)
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            try
            {
                var producto = await client.GetFromJsonAsync<ProductoModel>($"api/productos/{id}");
                if (producto == null) return NotFound();
                return View(producto);
            }
            catch
            {
                return NotFound();
            }
        }

        // ========================================================
        // GET: /Productos/Create
        // ========================================================
        public async Task<IActionResult> Create()
        {
            await CargarProveedoresAsync();
            return View(new ProductoModel());
        }

        // ========================================================
        // POST: /Productos/Create
        // ========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductoModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarProveedoresAsync();
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            var dto = new
            {
                id_producto = 0,
                id_proveedor = model.Id_proveedor,
                nombre = model.Nombre,
                tipo_venta = model.Tipo_venta,
                precio_unitario = model.Precio_unitario,
                stock_actual = model.Stock_actual,
                stock_minimo = model.Stock_minimo
            };

            var response = await client.PostAsJsonAsync("api/productos", dto);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "No se pudo crear el producto en la API.";
                await CargarProveedoresAsync();
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // ========================================================
        // GET: /Productos/Edit/5
        // ========================================================
        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            try
            {
                var producto = await client.GetFromJsonAsync<ProductoModel>($"api/productos/{id}");
                if (producto == null) return NotFound();

                await CargarProveedoresAsync();
                return View(producto);
            }
            catch
            {
                return NotFound();
            }
        }

        // ========================================================
        // POST: /Productos/Edit/5
        // ========================================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductoModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarProveedoresAsync();
                return View(model);
            }

            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            var dto = new
            {
                id_producto = id,
                id_proveedor = model.Id_proveedor,
                nombre = model.Nombre,
                tipo_venta = model.Tipo_venta,
                precio_unitario = model.Precio_unitario,
                stock_actual = model.Stock_actual,
                stock_minimo = model.Stock_minimo
            };

            var response = await client.PutAsJsonAsync($"api/productos/{id}", dto);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "No se pudo actualizar el producto en la API.";
                await CargarProveedoresAsync();
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // ========================================================
        // GET: /Productos/Delete/5
        // ========================================================
        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            try
            {
                var producto = await client.GetFromJsonAsync<ProductoModel>($"api/productos/{id}");
                if (producto == null) return NotFound();
                return View(producto);
            }
            catch
            {
                return NotFound();
            }
        }

        // ========================================================
        // POST: /Productos/Delete/5
        // ========================================================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            try
            {
                await client.DeleteAsync($"api/productos/{id}");
            }
            catch
            {
                ViewBag.Error = "No se pudo eliminar el producto en la API.";
            }

            return RedirectToAction(nameof(Index));
        }

        // ========================================================
        // Helper: Cargar proveedores para dropdowns
        // ========================================================
        private async Task CargarProveedoresAsync()
        {
            var client = _httpClientFactory.CreateClient("KoreanStoreAPI");

            try
            {
                var proveedores = await client.GetFromJsonAsync<List<ProveedorModel>>("api/proveedores")
                                  ?? new List<ProveedorModel>();

                ViewBag.Proveedores = proveedores
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Nombre
                    })
                    .ToList();
            }
            catch
            {
                ViewBag.Proveedores = new List<SelectListItem>();
            }
        }
    }
}