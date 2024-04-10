using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Data.SqlClient;
using MinimalAPICurso.DTOs;
using MinimalAPICurso.Entidades;
using MinimalAPICurso.Repositorios;
using MinimalAPICurso.Servicios;

namespace MinimalAPICurso.Endpoints
{
    public static class ActoresEndpoints
    {
        // Carpeta o contenedor en la nube / local
        private static readonly string contenedor = "actores";

        public static RouteGroupBuilder MapActores(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerActores).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("actores-get"));
            group.MapGet("/{id:int}", ObtenerActorPorId);
            group.MapGet("obtenerPorNombre/{nombre}", ObtenerActorPorNombre);
            group.MapPost("/", CrearActor).DisableAntiforgery();
            return group;
        }

        static async Task<Ok<List<ActorDTO>>> ObtenerActores(IRepositorioActores repositorioActores, IMapper mapper, int pagina = 1, int registrosPorPagina = 10)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RegistrosPorPagina = registrosPorPagina };
            var actores = await repositorioActores.ObtenerActores(paginacion);
            var actoresDTO = mapper.Map<List<ActorDTO>>(actores);
            return TypedResults.Ok(actoresDTO);
        }

        static async Task<Results<Ok<ActorDTO>, NotFound>> ObtenerActorPorId(int id, IRepositorioActores repositorioActores, IMapper mapper)
        {
            var actor = await repositorioActores.ObtenerActorPorId(id);

            if (actor is null)
            {
                return TypedResults.NotFound();
            }

            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Ok(actorDTO);
        }

        static async Task<Ok<List<ActorDTO>>> ObtenerActorPorNombre(string nombre, IRepositorioActores repositorioActores, IMapper mapper)
        {
            var actores = await repositorioActores.ObtenerActorPorNombre(nombre);
            var actoresDTO = mapper.Map<List<ActorDTO>>(actores);
            return TypedResults.Ok(actoresDTO);
        }

        static async Task<Created<ActorDTO>> CrearActor([FromForm] CrearActorDTO crearActorDTO, IRepositorioActores repositorioActores, IOutputCacheStore outputCacheStore, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            var actor = mapper.Map<Actor>(crearActorDTO);

            if (crearActorDTO.Foto is not null)
            {
                var url = await almacenadorArchivos.AlmacenarArchivo(contenedor, crearActorDTO.Foto);
                actor.Foto = url;
            }

            var id = await repositorioActores.CrearActor(actor);
            await outputCacheStore.EvictByTagAsync("actores-get", default);
            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Created($"/actores/{id}", actorDTO);
        }
    }
}