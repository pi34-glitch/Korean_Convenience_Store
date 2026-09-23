using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoreanStoreApi.Models
{
    public class Producto
    {
        [Key]
        public int Id_producto { get; set; }

        public int Id_proveedor { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public TipoVenta Tipo_venta { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio_unitario { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Stock_actual { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Stock_minimo { get; set; }

        // Navegación
        [ForeignKey(nameof(Id_proveedor))]
        public Proveedor? Proveedor { get; set; }

        public ICollection<Reseña> Reseñas { get; set; } = new List<Reseña>();
        public ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();
        public ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();
    }
}