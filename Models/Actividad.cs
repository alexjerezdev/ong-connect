using System.ComponentModel.DataAnnotations;

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
        public virtual Proyecto Proyecto { get; set; } = null!;
    }
}