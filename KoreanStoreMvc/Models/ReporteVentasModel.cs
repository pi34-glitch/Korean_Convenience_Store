namespace KoreanStoreMvc.Models
{
    public class ReporteVentasModel
    {
        public string Fecha { get; set; } = string.Empty;
        public decimal TotalVentas { get; set; }
        public decimal Efectivo { get; set; }
        public decimal Qr { get; set; }
        public decimal Tarjeta { get; set; }
    }
}