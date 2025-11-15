namespace minimalapi.Entidades
{
    public class Comentario
    {
        public int Id { get; set; }
        public String Cuerpo { get; set; } = null!;
        public int PeliculaId { get; set; }
    }
}
