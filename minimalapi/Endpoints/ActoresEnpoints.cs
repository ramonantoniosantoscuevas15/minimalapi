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
            return group;
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
