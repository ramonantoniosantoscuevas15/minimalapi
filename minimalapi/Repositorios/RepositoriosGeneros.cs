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
    }
}
