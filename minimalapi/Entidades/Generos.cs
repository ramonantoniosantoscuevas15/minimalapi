using System.ComponentModel.DataAnnotations;

namespace minimalapi.Entidades
{
    public class Generos
    {
        public int Id { get; set; }
        
        public String Nombre { get; set; } = null!;
    }
}
