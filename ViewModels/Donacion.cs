using System.ComponentModel.DataAnnotations;

namespace ONG_connect.ViewModels
{

    public class DonacionCreateViewModel
    {
        [Required(ErrorMessage = "El nombre del donante es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Debe tener entre 3 y 100 caracteres.")]
        public string NombreDonante { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecciona un tipo de donación.")]
        public string TipoDonacion { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 999999.99, ErrorMessage = "El valor debe ser un monto positivo.")]
        public decimal ValorEconomico { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaDonacion { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Debes asociar un proyecto.")]
        public int IdProyecto { get; set; }
    }
    


    public class DonacionEditViewModel : DonacionCreateViewModel
    {
        public int IdDonacion { get; set; }
    }
}