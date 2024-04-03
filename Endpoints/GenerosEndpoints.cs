using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
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

        static async Task<Ok<List<Genero>>> ObtenerGeneros(IRepositorioGeneros repositorioGeneros)
        {
            var generos = await repositorioGeneros.ObtenerGeneros();
            return TypedResults.Ok(generos);
        }

        static async Task<Results<Ok<Genero>, NotFound>> ObtenerGeneroPorId(int id, IRepositorioGeneros repositorioGeneros)
        {
            var genero = await repositorioGeneros.ObtenerGeneroPorId(id);

            if (genero is null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(genero);
        }

        static async Task<Created<Genero>> CrearGenero(Genero genero, IRepositorioGeneros repositorioGeneros, IOutputCacheStore outputCacheStore)
        {
            var id = await repositorioGeneros.CrearGenero(genero);
            await outputCacheStore.EvictByTagAsync("generos-get", default);
            return TypedResults.Created($"/generos/{id}", genero);
        }

        static async Task<Results<NoContent, NotFound>> ActualizarGenero(int id, Genero genero, IRepositorioGeneros repositorioGeneros, IOutputCacheStore outputCacheStore)
        {

            var existe = await repositorioGeneros.ExisteGenero(id);

            if (!existe)
            {
                return TypedResults.NotFound();
            }

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