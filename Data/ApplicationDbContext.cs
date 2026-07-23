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

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Proyecto> Proyectos { get; set; }
        public DbSet<Voluntario> Voluntarios { get; set; }
        public DbSet<Actividad> Actividades { get; set; }
        public DbSet<Beneficiario> Beneficiarios { get; set; }
        public DbSet<Donacion> Donaciones { get; set; }

         protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Actividad>()
        .Property(a => a.Fecha)
        .HasColumnType("timestamp without time zone");


    modelBuilder.Entity<Donacion>()
        .Property(d => d.FechaDonacion)
        .HasColumnType("timestamp without time zone");
}
    }
}