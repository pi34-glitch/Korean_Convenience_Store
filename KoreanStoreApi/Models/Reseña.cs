using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KoreanStoreApi.Models
{
    public class Reseña
    {
        [Key]
        public int Id { get; set; }

        public int Id_user { get; set; }

        public int Id_producto { get; set; }

        [Range(1, 5)]
        public int Calificacion { get; set; }

        public string? Comentario { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // Navegación
        [ForeignKey(nameof(Id_user))]
        public Usuario? Usuario { get; set; }

        [ForeignKey(nameof(Id_producto))]
        public Producto? Producto { get; set; }
    }
}