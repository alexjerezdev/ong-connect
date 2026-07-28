using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ONG_connect.Models
{
    public class Actividad
    {
        [Key]
        public int IdActividad { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        public DateTime Fecha { get; set; }

        public string Responsable { get; set; } = string.Empty;

        public int IdProyecto { get; set; }

        [ForeignKey("IdProyecto")]
        [ValidateNever]
        public virtual Proyecto Proyecto { get; set; } = null!;
    }
}