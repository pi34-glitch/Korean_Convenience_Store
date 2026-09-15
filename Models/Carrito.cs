using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Korean_Convenience_Store.Models
{
    public class Carrito
    {
        [Key]
        [Column("Id_carrito")]
        public int Id { get; set; }

        [Required]
        [Column("Id_user")]
        public int IdUser { get; set; }

        [Required]
        [Column("Id_producto")]
        public int IdProducto { get; set; }

        /// <summary>
        /// Cantidad para productos vendidos por unidad (empaquetados: ramen, bebidas, snacks).
        /// Es 0 si el producto se vende por peso.
        /// </summary>
        [Column("CantidadUnitario")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad unitaria no puede ser negativa")]
        public int CantidadUnitario { get; set; }

        /// <summary>
        /// Cantidad para productos vendidos por peso (preparados/autoservicio).
        /// Es 0 si el producto se vende por unidad.
        /// </summary>
        [Column("CantidadGramo", TypeName = "decimal(10,2)")]
        [Range(0, 99999999.99, ErrorMessage = "La cantidad en gramos no puede ser negativa")]
        public decimal CantidadGramo { get; set; }

        [Required]
        [Column("Subtotal", TypeName = "decimal(10,2)")]
        [Range(0, 99999999.99)]
        public decimal Subtotal { get; set; }

        // ===== Navegación =====
        [ForeignKey(nameof(IdUser))]
        public Usuario? Usuario { get; set; }

        [ForeignKey(nameof(IdProducto))]
        public Producto? Producto { get; set; }
    }
}