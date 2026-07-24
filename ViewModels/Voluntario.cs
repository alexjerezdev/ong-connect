using System.ComponentModel.DataAnnotations;

namespace ONG_connect.ViewModels
{
    public class VoluntarioCreateViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El teléfono no es válido.")]
        public string Telefono { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "El correo no es válido.")]
        public string Email { get; set; } = string.Empty;

        public bool Estado { get; set; }

        [Required(ErrorMessage = "Debes asociar un proyecto.")]
        public int IdProyecto { get; set; }
    }

    public class VoluntarioEditViewModel : VoluntarioCreateViewModel
    {
        public int IdVoluntario { get; set; }
    }
}