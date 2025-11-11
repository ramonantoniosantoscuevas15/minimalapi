using Microsoft.EntityFrameworkCore;
using minimalapi.Entidades;

namespace minimalapi.Repositorios
{
    public class RepositoriosGeneros : IRepositorioGeneros
    {
        private readonly AplicationDbContext context;

        public RepositoriosGeneros(AplicationDbContext context) 
        {
            this.context = context;
        }

       

        public async Task<int> Crear(Generos genero)
        {
            context.Add(genero);
            await context.SaveChangesAsync();
            return genero.Id;
        }
        public async Task Actualizar(Generos genero)
        {
            context.Update(genero);
            await context.SaveChangesAsync();
        }

        public async Task<bool> Existe(int id)
        {
            return await context.Generos.AnyAsync(x =>  x.Id == id);
        }

        public async Task<Generos?> ObtenerPorId(int id)
        {
            return await context.Generos.FirstOrDefaultAsync(x => x.Id ==id);
        }

        public async Task<List<Generos>> ObtenerTodos()
        {
            return await context.Generos.OrderBy(x =>x.Nombre).ToListAsync();
        }

        public async Task Borrar(int id)
        {
            await context.Generos.Where(x => x.Id == id).ExecuteDeleteAsync();
        }
    }
}
