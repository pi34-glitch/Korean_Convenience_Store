using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoreanStoreApi.Models
{
    public class DetalleVenta
    {
        [Key]
        public int Id_detalle { get; set; }

        public int Id_venta { get; set; }

        public int Id_producto { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Cantidad_o_gramos { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio_aplicado { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }

        // Navegación
        [ForeignKey(nameof(Id_venta))]
        public Venta? Venta { get; set; }

        [ForeignKey(nameof(Id_producto))]
        public Producto? Producto { get; set; }
    }
}