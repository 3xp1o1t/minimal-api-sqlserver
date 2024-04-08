using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
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
            group.MapPost("/", CrearActor).DisableAntiforgery();
            return group;
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