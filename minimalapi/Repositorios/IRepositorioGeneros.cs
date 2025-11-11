using minimalapi.Entidades;

namespace minimalapi.Repositorios
{
    public interface IRepositorioGeneros
    {
     

        Task<int> Crear(Generos genero);
    }
}
