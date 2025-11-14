namespace minimalapi.DTOs
{
    public class PeliculaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public bool Encines { get; set; }

        public DateTime FechaLanzamiento { get; set; }

        public string? Poster { get; set; }
    }
}
