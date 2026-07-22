using Microsoft.EntityFrameworkCore;
using ONG_connect.Models;

namespace ONG_connect.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarioso { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Voluntario> Voluntarios { get; set; }
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Beneficiario> Beneficiarios { get; set; }
        public DbSet<Donacion> Donaciones { get; set; }
    }
}