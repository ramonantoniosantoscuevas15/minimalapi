using minimalapi.Entidades;

namespace minimalapi.Repositorios
{
    public interface IRepositorioGeneros
    {

        Task<List<Generos>> ObtenerTodos();
        Task<Generos?> ObtenerPorId(int id);
        Task<int> Crear(Generos genero);
        Task<bool>Existe(int id);
        Task Actualizar(Generos genero);
        Task Borrar(int id);
        Task<List<int>> Existen(List<int> ids);
    }
}
