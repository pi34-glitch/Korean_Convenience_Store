namespace KoreanStoreApi.DTOs
{
    public class DetalleVentaDto
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