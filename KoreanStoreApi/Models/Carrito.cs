using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoreanStoreApi.Models
{
    public class Carrito
    {
        [Key]
        public int Id_carrito { get; set; }

        public int Id_user { get; set; }

        public int Id_producto { get; set; }

        public int CantidadUnitario { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CantidadGramo { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }

        // Navegación
        [ForeignKey(nameof(Id_user))]
        public Usuario? Usuario { get; set; }

        [ForeignKey(nameof(Id_producto))]
        public Producto? Producto { get; set; }
    }
}