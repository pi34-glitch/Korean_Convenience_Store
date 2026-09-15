using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Korean_Convenience_Store.Models
{
    public class DetalleVenta
    {
        [Key]
        [Column("Id_detalle")]
        public int Id { get; set; }

        [Required]
        [Column("Id_venta")]
        public int IdVenta { get; set; }

        [Required]
        [Column("Id_producto")]
        public int IdProducto { get; set; }

        /// <summary>
        /// Cantidad o gramos del producto en el momento de la venta.
        /// Se guarda como decimal(10,2) para unificar ambos tipos de venta.
        /// </summary>
        [Required]
        [Column("Cantidad_o_gramos", TypeName = "decimal(10,2)")]
        [Range(0.01, 99999999.99, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public decimal CantidadOGramos { get; set; }

        [Required]
        [Column("Precio_aplicado", TypeName = "decimal(10,2)")]
        [Range(0.01, 99999999.99)]
        public decimal PrecioAplicado { get; set; }

        [Required]
        [Column("Subtotal", TypeName = "decimal(10,2)")]
        [Range(0.01, 99999999.99)]
        public decimal Subtotal { get; set; }

        // ===== Navegación =====
        [ForeignKey(nameof(IdVenta))]
        public Venta? Venta { get; set; }

        [ForeignKey(nameof(IdProducto))]
        public Producto? Producto { get; set; }
    }
}