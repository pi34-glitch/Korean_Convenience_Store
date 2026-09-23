using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoreanStoreApi.Models
{
    public class Venta
    {
        [Key]
        public int Id_venta { get; set; }

        public int Id_user { get; set; }

        public DateTime Fecha_venta { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        [Required]
        public MetodoPago Metodo_pago { get; set; }

        // Navegación
        [ForeignKey(nameof(Id_user))]
        public Usuario? Usuario { get; set; }

        public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
    }
}