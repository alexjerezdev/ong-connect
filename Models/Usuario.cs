using System.ComponentModel.DataAnnotations;

namespace ONG_connect.Models
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Rol { get; set; } = string.Empty;

        // Relación 1:N
        public virtual ICollection<Proyecto> Proyectos { get; set; } = new HashSet<Proyecto>();
    }
}