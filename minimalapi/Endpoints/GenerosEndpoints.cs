using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.AspNetCore.Routing;
using minimalapi.DTOs;
using minimalapi.Entidades;
using minimalapi.Repositorios;

namespace minimalapi.Endpoints
{
    public static class GenerosEndpoints
    {
        public static RouteGroupBuilder MapGeneros(this RouteGroupBuilder group) {
            group.MapGet("/", ObtenerGenenros).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("generos-get"));

            group.MapGet("/{id:int}", ObtenerGenenroporId);

            group.MapPost("/", CrearGenero);

            group.MapPut("/{id:int}", Actualizargenero);

            group.MapDelete("/{id:int}", Borrargenero);
            return group;
        }
        static async Task<Ok<List<GeneroDTO>>> ObtenerGenenros(IRepositorioGeneros repositorio, IMapper mapper)
        {

            var generos = await repositorio.ObtenerTodos();
            var generosDTO = generos.Select(x => new GeneroDTO { Id = x.Id,Nombre = x.Nombre}).ToList();
            return TypedResults.Ok(generosDTO);
        }

        static async Task<Results<Ok<GeneroDTO>, NotFound>> ObtenerGenenroporId(IRepositorioGeneros repositorio, int id
            , IMapper mapper)
        {
            var genero = await repositorio.ObtenerPorId(id);

            if (genero is null)
            {
                return TypedResults.NotFound();
            }
            var generoDTO = mapper.Map<GeneroDTO>(genero);
            return TypedResults.Ok(generoDTO);

        }

        static async Task<Created<GeneroDTO>> CrearGenero(CrearGeneroDTO crearGeneroDTO, 
            IRepositorioGeneros repositorio,
            IOutputCacheStore outputCacheStore, IMapper mapper)
        {

            var genero = mapper.Map<Generos>(crearGeneroDTO);
            var id = await repositorio.Crear(genero);
            await outputCacheStore.EvictByTagAsync("generos-get", default);
            var generoDTO = mapper.Map<GeneroDTO>(genero);
            return TypedResults.Created($"/generos/{id}", generoDTO);
        }

        static async Task<Results<NoContent, NotFound>> Actualizargenero(int id, 
            CrearGeneroDTO crearGeneroDTO, IRepositorioGeneros repositorio,
           IOutputCacheStore outputCacheStore, IMapper mapper
           )
        {
            var existe = await repositorio.Existe(id);

            if (!existe)
            {
                return TypedResults.NotFound();
            }
            var genero = mapper.Map<Generos>(crearGeneroDTO);
            genero.Id = id;
            await repositorio.Actualizar(genero);
            await outputCacheStore.EvictByTagAsync("generos-get", default);
            return TypedResults.NoContent();

        }

        static async Task<Results<NoContent, NotFound>> Borrargenero(int id, IRepositorioGeneros repositorio,
            IOutputCacheStore outputCacheStore)
        {
            var existe = await repositorio.Existe(id);

            if (!existe)
            {
                return TypedResults.NotFound();
            }
            await repositorio.Borrar(id);
            await outputCacheStore.EvictByTagAsync("generos-get", default);
            return TypedResults.NoContent();
        }
    }
}
