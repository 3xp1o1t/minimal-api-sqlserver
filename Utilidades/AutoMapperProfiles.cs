using AutoMapper;
using MinimalAPICurso.DTOs;
using MinimalAPICurso.Entidades;

namespace MinimalAPICurso.Utilidades
{
  public class AutoMapperProfiles : Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<CrearGeneroDTO, Genero>();
      CreateMap<Genero, GeneroDTO>();
    }
  }
}