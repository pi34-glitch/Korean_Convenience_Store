using System.ComponentModel.DataAnnotations;

namespace KoreanStoreApi.DTOs
{
    public class UsuarioLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}