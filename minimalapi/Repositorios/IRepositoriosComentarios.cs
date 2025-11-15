using minimalapi.Entidades;

namespace minimalapi.Repositorios
{
    public interface IRepositoriosComentarios
    {
        Task Actualizar(Comentario comentario);
        Task Borrar(int id);
        Task<int> Crear(Comentario comentario);
        Task<bool> Existe(int id);
        Task<Comentario?> ObtenerPorId(int id);
        Task<List<Comentario>> ObtenerTodos(int peliculaId);
    }
}