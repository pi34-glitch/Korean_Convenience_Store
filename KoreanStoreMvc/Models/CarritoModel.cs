namespace KoreanStoreMvc.Models
{
    public class CarritoModel
    {
        public int Id_carrito { get; set; }
        public int Id_user { get; set; }
        public int Id_producto { get; set; }
        public int CantidadUnitario { get; set; }
        public decimal CantidadGramo { get; set; }
        public decimal Subtotal { get; set; }
        public string? NombreProducto { get; set; }
    }
}