using System.ComponentModel.DataAnnotations;


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

        // Clave foránea
        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; } = null!;

        public virtual ICollection<Voluntario> Voluntarios { get; set; } = new HashSet<Voluntario>();
        public virtual ICollection<Actividad> Actividades { get; set; } = new HashSet<Actividad>();
        public virtual ICollection<Beneficiario> Beneficiarios { get; set; } = new HashSet<Beneficiario>();
        public virtual ICollection<Donacion> Donaciones { get; set; } = new HashSet<Donacion>();
    }
}