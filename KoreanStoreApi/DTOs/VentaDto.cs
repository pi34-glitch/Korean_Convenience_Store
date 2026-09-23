namespace KoreanStoreApi.DTOs
{
    public class VentaDto
    {
        public int Id_venta { get; set; }
        public int Id_user { get; set; }
        public DateTime Fecha_venta { get; set; }
        public decimal Total { get; set; }
        public string Metodo_pago { get; set; } = string.Empty;
        public List<DetalleVentaDto> Detalles { get; set; } = new();
    }
}