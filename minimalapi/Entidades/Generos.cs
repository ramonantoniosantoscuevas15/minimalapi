using System.ComponentModel.DataAnnotations;

namespace minimalapi.Entidades
{
    public class Generos
    {

        public int Id { get; set; }
        
        public String Nombre { get; set; } = null!;

        public List<GeneroPelicula> GeneroPeliculas { get; set; } = new List<GeneroPelicula>();
    }
}
