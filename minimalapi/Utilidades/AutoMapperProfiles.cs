using AutoMapper;
using minimalapi.DTOs;
using minimalapi.Entidades;

namespace minimalapi.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<CrearGeneroDTO, Generos>();
            CreateMap<Generos, GeneroDTO>();

            CreateMap<CrearActorDTO, Actor>()
                .ForMember(x => x.Foto, opciones => opciones.Ignore());
            CreateMap<Actor, ActorDTO>();

            CreateMap<CrearPeliculaDTO, Pelicula>()
               .ForMember(x => x.Poster, opciones => opciones.Ignore());
            CreateMap<Pelicula, PeliculaDTO>();

        }
    }
}
