using System.ComponentModel.DataAnnotations;

namespace ONG_connect.ViewModels
{
    public class BeneficiarioCreateViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tipo de beneficiario es obligatorio.")]
        public string TipoBeneficiario { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 999999.99, ErrorMessage = "La cantidad de ayuda debe ser un valor positivo.")]
        public decimal CantidadAyuda { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un proyecto.")]
        public int IdProyecto { get; set; }
    }

    public class BeneficiarioEditViewModel : BeneficiarioCreateViewModel
    {
        [Required]
        public int IdBeneficiario { get; set; }
    }
}