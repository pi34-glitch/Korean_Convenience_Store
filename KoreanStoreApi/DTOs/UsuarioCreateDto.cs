using System.ComponentModel.DataAnnotations;

namespace KoreanStoreApi.DTOs
{
    public class UsuarioCreateDto
    {
        [Required]
        [StringLength(100)]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Rol { get; set; } = "Cliente";
    }
}