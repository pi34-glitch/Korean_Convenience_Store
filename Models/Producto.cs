using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Korean_Convenience_Store.Models
{
    public class Producto
    {
        [Key]
        [Column("Id_producto")]
        public int Id { get; set; }

        [Required]
        [Column("Id_proveedor")]
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Column("Tipo_venta")]
        public TipoVenta TipoVenta { get; set; }

        [Column("Precio_unitario", TypeName = "decimal(10,2)")]
        [Range(0.01, 99999999.99, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal PrecioUnitario { get; set; }

        [Column("Stock_actual", TypeName = "decimal(10,2)")]
        [Range(0, 99999999.99)]
        public decimal StockActual { get; set; }

        [Column("Stock_minimo", TypeName = "decimal(10,2)")]
        [Range(0, 99999999.99)]
        public decimal StockMinimo { get; set; }

        // ===== Navegación =====
        [ForeignKey(nameof(IdProveedor))]
        public Proveedor? Proveedor { get; set; }

        public ICollection<Resena> Resenas { get; set; } = new List<Resena>();
        public ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();
        public ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();
    }
}