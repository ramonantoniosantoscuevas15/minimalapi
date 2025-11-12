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
        
        }
    }
}
