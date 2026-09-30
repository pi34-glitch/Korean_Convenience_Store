using KoreanStoreApi.Data;
using KoreanStoreApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KoreanStoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/reportes/ventas-diarias
        [HttpGet("ventas-diarias")]
        public async Task<IActionResult> VentasDiarias()
        {
            var hoy = DateTime.UtcNow.Date;
            var manana = hoy.AddDays(1);

            var ventas = await _context.Ventas
                .Where(v => v.Fecha_venta >= hoy &&
                            v.Fecha_venta < manana)
                .ToListAsync();

            var totalVentas = ventas.Sum(v => v.Total);

            var efectivo = ventas
                .Where(v => v.Metodo_pago == MetodoPago.Efectivo)
                .Sum(v => v.Total);

            var qr = ventas
                .Where(v => v.Metodo_pago == MetodoPago.QR)
                .Sum(v => v.Total);

            var tarjeta = ventas
                .Where(v => v.Metodo_pago == MetodoPago.Tarjeta)
                .Sum(v => v.Total);

            return Ok(new
            {
                fecha = hoy.ToString("yyyy-MM-dd"),
                totalVentas = totalVentas,
                efectivo = efectivo,
                qr = qr,
                tarjeta = tarjeta
            });
        }
    }
}