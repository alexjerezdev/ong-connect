using System.ComponentModel.DataAnnotations;

namespace ONG_connect.Models
{
    public class Donacion
    {
        [Key]
        public int IdDonacion { get; set; }

        [Required]
        public string NombreDonante { get; set; } = string.Empty;

        public string TipoDonacion { get; set; } = string.Empty;

        public decimal ValorEconomico { get; set; }

        public DateTime FechaDonacion { get; set; }

        public int IdProyecto { get; set; }
        public virtual Proyecto Proyecto { get; set; } = null!;
    }
}