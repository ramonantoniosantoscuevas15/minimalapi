using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using minimalapi.DTOs;
using minimalapi.Entidades;
using minimalapi.Migrations;
using minimalapi.Repositorios;

namespace minimalapi.Endpoints
{
    public static class ComentariosEndpoints
    {
        public static RouteGroupBuilder MapComentarios(this RouteGroupBuilder group) 
        {
            group.MapGet("/", ObtenerTodos).CacheOutput(c=> c.Expire
            (TimeSpan.FromSeconds(60)).Tag("comentarios-get"));
            group.MapGet("/{id:int}",ObtenerPorId).WithName("ObtenerComentarioPorId");
            group.MapPost("/",Crear);
            return group;
        }

        static async Task<Results<Ok<List<ComentarioDTO>>,NotFound>> ObtenerTodos(int peliculaId, IRepositoriosComentarios repositoriosComentarios,
            IRepositorioPeliculas repositorioPeliculas, IMapper mapper)
        {
            if (!await repositorioPeliculas.Existe(peliculaId))
            {
                return TypedResults.NotFound();
            }
            var comentarios = await repositoriosComentarios.ObtenerTodos(peliculaId);
            var comentarioDTO = mapper.Map<List<ComentarioDTO>>(comentarios);
            return TypedResults.Ok(comentarioDTO);

        }

        static async Task<Results<Ok<ComentarioDTO>, NotFound>> ObtenerPorId (int peliculaId,int id,
            IRepositoriosComentarios repositorios,IMapper mapper)
        {
            var comentario = await repositorios.ObtenerPorId(id);

            if(comentario is null)
            {
                return TypedResults.NotFound();
            }
            var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);
            return TypedResults.Ok(comentarioDTO);

        }
        

        static async Task<Results<CreatedAtRoute<ComentarioDTO>,NotFound>> Crear(int peliculaId, CrearComentarioDTO crearComentarioDTO,
            IRepositoriosComentarios repositoriosComentarios, IRepositorioPeliculas repositorioPeliculas,IMapper mapper, 
            IOutputCacheStore outputCacheStore)
        {
            if(! await repositorioPeliculas.Existe(peliculaId))
            {
                return TypedResults.NotFound();
            }

            var comentario = mapper.Map<Comentario>(crearComentarioDTO);
            comentario.PeliculaId = peliculaId;
            var id =await repositoriosComentarios.Crear(comentario);
            await outputCacheStore.EvictByTagAsync("comentarios-get",default);
            var comentariosDTO = mapper.Map<ComentarioDTO>(comentario);
            return TypedResults.CreatedAtRoute(comentariosDTO, "ObtenerComentarioPorId", new {id,peliculaId});
        }

    }
}
