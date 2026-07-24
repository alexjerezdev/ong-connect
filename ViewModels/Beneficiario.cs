using System.ComponentModel.DataAnnotations;

namespace ONG_connect.ViewModels
{
    public class BeneficiarioCreateViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecciona un tipo de beneficiario.")]
        public string TipoBeneficiario { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 999999.99, ErrorMessage = "La cantidad de ayuda debe ser un valor positivo.")]
        public decimal CantidadAyuda { get; set; }

        [Required(ErrorMessage = "Debes asociar un proyecto.")]
        public int IdProyecto { get; set; }
    }

    public class BeneficiarioEditViewModel : BeneficiarioCreateViewModel
    {
        public int IdBeneficiario { get; set; }
    }
}