using System.ComponentModel.DataAnnotations;

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
        public virtual Proyecto Proyecto { get; set; } = null!;
    }
}