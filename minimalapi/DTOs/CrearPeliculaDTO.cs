namespace minimalapi.DTOs
{
    public class CrearPeliculaDTO
    {
        public string Titulo { get; set; } = null!;
        public bool Encines { get; set; }

        public DateTime FechaLanzamiento { get; set; }

        public IFormFile? Poster { get; set; }
    }
}
