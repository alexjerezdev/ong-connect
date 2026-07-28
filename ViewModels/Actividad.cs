using System.ComponentModel.DataAnnotations;

namespace ONG_connect.ViewModels
{
    public class ActividadCreateViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "El responsable es obligatorio.")]
        public string Responsable { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debes asociar un proyecto.")]
        public int IdProyecto { get; set; }
    }

    public class ActividadEditViewModel : ActividadCreateViewModel
    {
        public int IdActividad { get; set; }
    }
}