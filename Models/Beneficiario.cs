using System.ComponentModel.DataAnnotations;

namespace ONG_connect.Models
{
    public class Beneficiario
    {
        [Key]
        public int IdBeneficiario { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        public string TipoBeneficiario { get; set; } = string.Empty;

        public decimal CantidadAyuda { get; set; }

        public int IdProyecto { get; set; }
        public virtual Proyecto Proyecto { get; set; } = null!;
    }
}