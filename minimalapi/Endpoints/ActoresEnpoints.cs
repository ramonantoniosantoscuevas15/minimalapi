using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using minimalapi.DTOs;
using minimalapi.Entidades;
using minimalapi.Repositorios;
using minimalapi.Servicios;

namespace minimalapi.Endpoints
{
    public static class ActoresEnpoints
    {
        private static readonly string contenedor = "actores";
        public static RouteGroupBuilder MapActores(this RouteGroupBuilder group) 
        {
            group.MapPost("/",Crear).DisableAntiforgery();
            group.MapGet("/",ObtenerTodos).CacheOutput
            (c=> c.Expire(TimeSpan.FromSeconds(60)).Tag("actores-get"));
            group.MapGet("/{id:int}",ObternerPorId);
            group.MapGet("obtenerpornombre/{nombre}", ObtenerPorNombre);
            return group;
        }
        static async Task<Ok<List<ActorDTO>>> ObtenerTodos(IRepositorioActores repositorioActores,IMapper mapper,
            int pagina=1,int recordsPorPagina =10)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RecordsPorPagina = recordsPorPagina };
            var actores = await repositorioActores.ObtenerTodos(paginacion);
            var actoresDTO = mapper.Map<List<ActorDTO>>(actores);
            return TypedResults.Ok(actoresDTO);

        }
        static async Task<Results<Ok<ActorDTO>, NotFound>> ObternerPorId(int id, IRepositorioActores repositorio, IMapper mapper)
        {
            var actor = await repositorio.ObtenerPorId(id);

            if (actor is null) 
            {
                return TypedResults.NotFound();
            
            }
            var actorDTO = mapper.Map<ActorDTO>(actor); 
            return TypedResults.Ok(actorDTO); 


        }
        static async Task<Ok<List<ActorDTO>>> ObtenerPorNombre(string nombre,
            IRepositorioActores repositorioActores, 
            IMapper mapper)
        {
            var actores = await repositorioActores.ObtenerPorNombre(nombre);
            var actoresDTO = mapper.Map<List<ActorDTO>>(actores);
            return TypedResults.Ok(actoresDTO);

        }
        static async Task<Created<ActorDTO>> Crear([FromForm] CrearActorDTO crearActorDTO, 
            IRepositorioActores repositorio, IOutputCacheStore outputCacheStore, IMapper mapper,
            IAlmacenadorArchivos almacenadorArchivos
            )
        {
            var actor = mapper.Map<Actor>( crearActorDTO );
            if (crearActorDTO.Foto is not null) 
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, crearActorDTO.Foto);
                actor.Foto = url;
            }
            var id = await repositorio.Crear( actor );
            await outputCacheStore.EvictByTagAsync("actores-get", default);
            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Created($"/actores/{id}", actorDTO);

        }

    }
}
