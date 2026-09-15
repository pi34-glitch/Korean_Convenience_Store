using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Korean_Convenience_Store.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [StringLength(100)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public RolUsuario Rol { get; set; }

        public bool Activo { get; set; } = true;

        // ===== Navegación (se llenará en fases siguientes) =====
        public ICollection<Resena> Resenas { get; set; } = new List<Resena>();
        public ICollection<Carrito> Carritos { get; set; } = new List<Carrito>();
        public ICollection<Venta> Ventas { get; set; } = new List<Venta>();
    }
}