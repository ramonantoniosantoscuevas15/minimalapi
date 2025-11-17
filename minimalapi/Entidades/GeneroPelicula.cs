namespace minimalapi.Entidades
{
    public class GeneroPelicula
    {
        public int PeliculaId { get; set; }
        public int GeneroId { get; set; }
        public Generos Genero { get; set; } = null!;
        public Pelicula pelicula { get; set; } = null!;
    }
}
