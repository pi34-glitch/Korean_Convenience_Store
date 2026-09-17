using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Korean_Convenience_Store.Models
{
    public class Venta
    {
        [Key]
        [Column("Id_venta")]
        public int Id { get; set; }

        [Required]
        [Column("Id_user")]
        public int IdUser { get; set; }

        [Required]
        [Column("Fecha_venta")]
        public DateTime FechaVenta { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("Total", TypeName = "decimal(10,2)")]
        [Range(0.01, 99999999.99, ErrorMessage = "El total debe ser mayor a 0")]
        public decimal Total { get; set; }

        [Required]
        [Column("Metodo_pago")]
        public MetodoPago MetodoPago { get; set; }

        // ===== Navegación =====
        [ForeignKey(nameof(IdUser))]
        public Usuario? Usuario { get; set; }

        public ICollection<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();
    }
}