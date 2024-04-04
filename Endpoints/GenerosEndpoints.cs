using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using MinimalAPICurso.DTOs;
using MinimalAPICurso.Entidades;
using MinimalAPICurso.Repositorios;

namespace MinimalAPICurso.Endpoints
{
    public static class GenerosEndpoints
    {
        public static RouteGroupBuilder MapGeneros(this RouteGroupBuilder group)
        {

            // Generos con cache habilitado
            group.MapGet("/", ObtenerGeneros).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("generos-get"));
            group.MapGet("/{id}", ObtenerGeneroPorId);
            group.MapPost("/", CrearGenero);
            group.MapPut("/{id:int}", ActualizarGenero);
            group.MapDelete("/{id:int}", BorrarGenero);

            return group;
        }

        static async Task<Ok<List<GeneroDTO>>> ObtenerGeneros(IRepositorioGeneros repositorioGeneros, IMapper mapper)
        {
            var generos = await repositorioGeneros.ObtenerGeneros();
            var generosDTO = mapper.Map<List<GeneroDTO>>(generos);
            return TypedResults.Ok(generosDTO);
        }

        static async Task<Results<Ok<GeneroDTO>, NotFound>> ObtenerGeneroPorId(int id, IRepositorioGeneros repositorioGeneros, IMapper mapper)
        {
            var genero = await repositorioGeneros.ObtenerGeneroPorId(id);

            if (genero is null)
            {
                return TypedResults.NotFound();
            }

            var generoDTO = mapper.Map<GeneroDTO>(genero);

            return TypedResults.Ok(generoDTO);
        }

        static async Task<Created<GeneroDTO>> CrearGenero(CrearGeneroDTO crearGeneroDTO, IRepositorioGeneros repositorioGeneros, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var genero = mapper.Map<Genero>(crearGeneroDTO);
            var id = await repositorioGeneros.CrearGenero(genero);
            await outputCacheStore.EvictByTagAsync("generos-get", default);
            var generoDTO = mapper.Map<GeneroDTO>(genero);

            return TypedResults.Created($"/generos/{id}", generoDTO);
        }

        static async Task<Results<NoContent, NotFound>> ActualizarGenero(int id, CrearGeneroDTO crearGeneroDTO, IRepositorioGeneros repositorioGeneros, IOutputCacheStore outputCacheStore, IMapper mapper)
        {

            var existe = await repositorioGeneros.ExisteGenero(id);

            if (!existe)
            {
                return TypedResults.NotFound();
            }

            var genero = mapper.Map<Genero>(crearGeneroDTO);
            genero.Id = id;

            await repositorioGeneros.ActualizarGenero(genero);

            await outputCacheStore.EvictByTagAsync("generos-get", default);
            return TypedResults.NoContent();

        }

        static async Task<Results<NoContent, NotFound>> BorrarGenero(int id, IRepositorioGeneros repositorioGeneros, IOutputCacheStore outputCacheStore)
        {
            var existe = await repositorioGeneros.ExisteGenero(id);

            if (!existe)
            {
                return TypedResults.NotFound();
            }

            await repositorioGeneros.BorrarGenero(id);
            await outputCacheStore.EvictByTagAsync("generos-get", default);
            return TypedResults.NoContent();
        }
    }
}