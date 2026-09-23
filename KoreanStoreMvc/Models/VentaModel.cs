namespace KoreanStoreMvc.Models
{
    public class VentaModel
    {
        public int Id_venta { get; set; }
        public int Id_user { get; set; }
        public DateTime Fecha_venta { get; set; }
        public decimal Total { get; set; }
        public string Metodo_pago { get; set; } = string.Empty;
        public List<DetalleVentaModel> Detalles { get; set; } = new();
    }

    public class DetalleVentaModel
    {
        public int Id_detalle { get; set; }
        public int Id_venta { get; set; }
        public int Id_producto { get; set; }
        public decimal Cantidad_o_gramos { get; set; }
        public decimal Precio_aplicado { get; set; }
        public decimal Subtotal { get; set; }
        public string? NombreProducto { get; set; }
    }
}