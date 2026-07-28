using System.ComponentModel.DataAnnotations;

namespace ONG_connect.ViewModels
{
    public class ProyectoCreateViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El responsable es obligatorio.")]
        public string Responsable { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 9999999.99, ErrorMessage = "El presupuesto debe ser un valor positivo.")]
        public decimal Presupuesto { get; set; }

        [Required(ErrorMessage = "Selecciona un estado.")]
        public string Estado { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debes asociar un usuario responsable.")]
        public int IdUsuario { get; set; }
    }

    public class ProyectoEditViewModel : ProyectoCreateViewModel
    {
        public int IdProyecto { get; set; }
    }
}