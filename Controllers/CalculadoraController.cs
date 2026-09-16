using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Korean_Convenience_Store.Data;
using Korean_Convenience_Store.Models;

namespace Korean_Convenience_Store.Controllers;

public class CalculadoraController : Controller
{
    private readonly ApplicationDbContext _context;

    public CalculadoraController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Calculadora
    public IActionResult Index()
    {
        var productos = _context.Productos.ToList();

        ViewBag.Productos = productos;

        return View(new Carrito());
    }

    // POST: /Calculadora
    [HttpPost]
    public IActionResult Index(Carrito carrito)
    {
        // Buscar el producto seleccionado
        var producto = _context.Productos
            .FirstOrDefault(p => p.Id == carrito.IdProducto);

        // Comprobar que el producto exista
        if (producto == null)
        {
            ModelState.AddModelError(
                "",
                "El producto seleccionado no existe."
            );

            ViewBag.Productos = _context.Productos.ToList();

            return View(carrito);
        }

        // Comprobar que los gramos sean mayores que 0
        if (carrito.CantidadGramo <= 0)
        {
            ModelState.AddModelError(
                "",
                "La cantidad de gramos debe ser mayor que 0."
            );

            ViewBag.Productos = _context.Productos.ToList();

            return View(carrito);
        }

        // Realizar el cálculo
        carrito.Subtotal =
            producto.PrecioUnitario * carrito.CantidadGramo;

        // Enviar el producto a la vista
        ViewBag.ProductoSeleccionado = producto;

        // Enviar los productos para el <select>
        ViewBag.Productos = _context.Productos.ToList();

        return View(carrito);
    }
}