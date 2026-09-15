using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Korean_Convenience_Store.Models
{
    public class Resena
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("Id_user")]
        public int IdUser { get; set; }

        [Required]
        [Column("Id_producto")]
        public int IdProducto { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5")]
        public int Calificacion { get; set; }

        [Column(TypeName = "text")]
        public string? Comentario { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // ===== Navegación =====
        [ForeignKey(nameof(IdUser))]
        public Usuario? Usuario { get; set; }

        [ForeignKey(nameof(IdProducto))]
        public Producto? Producto { get; set; }
    }
}