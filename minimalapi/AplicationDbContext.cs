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
            modelBuilder.Entity<Actor>().Property(p => p.Nombre).HasMaxLength(150);
            modelBuilder.Entity<Actor>().Property(P=>P.Foto).IsUnicode(false);
            modelBuilder.Entity<Pelicula>().Property(p=> p.Titulo).HasMaxLength(150);
            modelBuilder.Entity<Pelicula>().Property(p => p.Poster).IsUnicode(false);
            modelBuilder.Entity<GeneroPelicula>().HasKey(g => new {g.GeneroId,g.PeliculaId});
        }
        public DbSet<Entidades.Generos> Generos {  get; set; }
        public DbSet<Actor>Actores { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<GeneroPelicula>GenerosPeliculas { get; set; }

        protected AplicationDbContext()
        {
        }
    }
}
