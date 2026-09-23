using System.ComponentModel.DataAnnotations;

namespace KoreanStoreApi.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public RolUsuario Rol { get; set; }

        public bool Activo { get; set; } = true;

        // Navegación
        public ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
        public ICollection<Reseña> Reseñas { get; set; } = new List<Reseña>();
    }
}