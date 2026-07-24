using System.ComponentModel.DataAnnotations;

namespace ONG_connect.ViewModels
{
    public class UsuarioCreateViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo no es válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Debe tener al menos 6 caracteres.")]
        public string Password { get; set; } = string.Empty;

        // Rol NO está aquí: no lo decide el usuario que se registra.
    }

    public class UsuarioEditViewModel
    {
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Rol { get; set; } = string.Empty;

        // Contraseña opcional: solo se cambia si se llena este campo.
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Debe tener al menos 6 caracteres.")]
        public string? NuevaPassword { get; set; }
    }
}