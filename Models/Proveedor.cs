using System.ComponentModel.DataAnnotations;

namespace Korean_Convenience_Store.Models
{
    public class Proveedor
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del proveedor es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(20)]
        public string? Contacto { get; set; }

        [StringLength(20)]
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        public string? Telefono { get; set; }

        [StringLength(150)]
        public string? Direccion { get; set; }

        // ===== Navegación =====
        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}