using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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

        [ForeignKey("IdProyecto")]
        [ValidateNever]
        public virtual Proyecto Proyecto { get; set; } = null!;
    }
}