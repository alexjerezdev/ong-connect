using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ONG_connect.Models
{
    public class Proyecto
    {
        [Key]
        public int IdProyecto { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Responsable { get; set; } = string.Empty;

        public decimal Presupuesto { get; set; }

        public string Estado { get; set; } = string.Empty;

        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        [ValidateNever]
        public virtual Usuario Usuario { get; set; } = null!;

        [ValidateNever]
        public virtual ICollection<Voluntario> Voluntarios { get; set; } = new HashSet<Voluntario>();

        [ValidateNever]
        public virtual ICollection<Actividad> Actividades { get; set; } = new HashSet<Actividad>();

        [ValidateNever]
        public virtual ICollection<Beneficiario> Beneficiarios { get; set; } = new HashSet<Beneficiario>();

        [ValidateNever]
        public virtual ICollection<Donacion> Donaciones { get; set; } = new HashSet<Donacion>();
    }
}