using Microsoft.EntityFrameworkCore;
using minimalapi.DTOs;
using minimalapi.Entidades;
using minimalapi.Utilidades;

namespace minimalapi.Repositorios
{
    public class RepositorioActores : IRepositorioActores
    {
        private readonly AplicationDbContext context;
        private readonly HttpContext httpContext;

        public RepositorioActores(AplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            httpContext = httpContextAccessor.HttpContext!;

        }
        public async Task<List<Actor>> ObtenerTodos(PaginacionDTO paginacionDTO)
        {
            var queryable = context.Actores.AsQueryable();
            await httpContext.InsertarParametrosPaginacionEncabecera(queryable);
            return await context.Actores.OrderBy(a => a.Nombre).ToListAsync();
        }

        public async Task<Actor?> ObtenerPorId(int id)
        {
            return await context.Actores.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Actor>> ObtenerPorNombre(string nombre)
        {
            return await context.Actores.Where(a => a.Nombre
            .Contains(nombre)).
            OrderBy(a => a.Nombre).
            ToListAsync();
        }

        public async Task<int> Crear(Actor actor)
        {
            context.Add(actor);
            await context.SaveChangesAsync();
            return actor.Id;
        }

        public async Task<bool> Existe(int id)
        {
            return await context.Actores.AnyAsync(a => a.Id == id);
        }

        public async Task Actualizar(Actor actor)
        {
            context.Update(actor);
            await context.SaveChangesAsync();
        }

        public async Task Borrar(int id)
        {
            await context.Actores.Where(a => a.Id == id).ExecuteDeleteAsync();
        }
    }
}
