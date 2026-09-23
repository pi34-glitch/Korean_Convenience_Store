namespace KoreanStoreApi.DTOs
{
    public class ProductoDto
    {
        public int Id_producto { get; set; }
        public int Id_proveedor { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Tipo_venta { get; set; } = string.Empty;
        public decimal Precio_unitario { get; set; }
        public decimal Stock_actual { get; set; }
        public decimal Stock_minimo { get; set; }
        public string? NombreProveedor { get; set; }
    }
}