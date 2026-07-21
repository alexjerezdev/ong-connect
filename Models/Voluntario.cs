using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ONG_connect.Models
{
    public class Voluntario
    {
        [Key]
        public int IdVoluntario { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public bool Estado { get; set; }

        public int IdProyecto { get; set; }

        [ForeignKey("IdProyecto")]
        [ValidateNever]
        public virtual Proyecto Proyecto { get; set; } = null!;
    }
}