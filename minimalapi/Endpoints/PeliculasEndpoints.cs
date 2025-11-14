using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using minimalapi.DTOs;
using minimalapi.Entidades;
using minimalapi.Repositorios;
using minimalapi.Servicios;
using System.Reflection.Metadata;

namespace minimalapi.Endpoints
{
    public static class PeliculasEndpoints
    {
        private static readonly string contenedor = "Peliculas";
        public static RouteGroupBuilder MapPeliculas(this RouteGroupBuilder group) 
        {
            group.MapPost("/", Crear).DisableAntiforgery();
            return group;


        }

        static async Task<Created<PeliculaDTO>> Crear([FromForm] CrearPeliculaDTO crearPeliculaDTO,
            IRepositorioPeliculas repositorio, IAlmacenadorArchivos almacenadorArchivos,
            IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var pelicula = mapper.Map<Pelicula>(crearPeliculaDTO);

            if (crearPeliculaDTO.Poster is not null)
            {
                var url = await almacenadorArchivos.Almacenar(contenedor, crearPeliculaDTO.Poster);
                pelicula.Poster = url;
            }

            var id = await repositorio.Crear(pelicula);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);
            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);
            return TypedResults.Created($"/peliculas/{id}", peliculaDTO);
        }
        
    }
}
