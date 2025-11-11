using Microsoft.EntityFrameworkCore;
using minimalapi.Entidades;
using minimalapi.Migrations;

namespace minimalapi
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Entidades.Generos>().Property(p => p.Nombre).HasMaxLength(50);
        }
        public DbSet<Entidades.Generos> Generos {  get; set; }

        protected AplicationDbContext()
        {
        }
    }
}
