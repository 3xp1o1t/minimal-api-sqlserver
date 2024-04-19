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
    public static class ComentariosEndpoints
    {
        public static RouteGroupBuilder MapComentarios(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerComentarios).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("comentarios-get").SetVaryByRouteValue(new string[] { "peliculaId" }));
            group.MapGet("/{id:int}", ObtenerComentarioPorId);
            group.MapPost("/", CrearComentario);
            group.MapPut("/{id:int}", ActualizarComentario);
            group.MapDelete("/{id:int}", BorrarComentario);
            return group;
        }

        static async Task<Results<Ok<List<ComentarioDTO>>, NotFound>> ObtenerComentarios(int peliculaId, IRepositorioComentarios repositorioComentarios, IRepositorioPeliculas repositorioPeliculas, IMapper mapper)
        {
            if (!await repositorioPeliculas.ExistePelicula(peliculaId))
            {
                return TypedResults.NotFound();
            }

            var comentarios = await repositorioComentarios.ObtenerComentarios(peliculaId);
            var comentariosDTO = mapper.Map<List<ComentarioDTO>>(comentarios);
            return TypedResults.Ok(comentariosDTO);
        }

        static async Task<Results<Ok<ComentarioDTO>, NotFound>> ObtenerComentarioPorId(int peliculaId, int id, IRepositorioComentarios repositorioComentarios, IMapper mapper)
        {
            var comentario = await repositorioComentarios.ObtenerComentarioPorId(id);

            if (comentario is null)
            {
                return TypedResults.NotFound();
            }

            var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);
            return TypedResults.Ok(comentarioDTO);
        }

        static async Task<Results<Created<ComentarioDTO>, NotFound>> CrearComentario(int peliculaId, CrearComentarioDTO crearComentarioDTO, IRepositorioComentarios repositorioComentarios, IRepositorioPeliculas repositorioPeliculas, IMapper mapper, IOutputCacheStore outputCacheStore)
        {
            if (!await repositorioPeliculas.ExistePelicula(peliculaId))
            {
                return TypedResults.NotFound();
            }

            var comentario = mapper.Map<Comentario>(crearComentarioDTO);
            comentario.PeliculaId = peliculaId;
            var id = await repositorioComentarios.CrearComentario(comentario);
            await outputCacheStore.EvictByTagAsync("comentarios-get", default);
            var comentarioDTO = mapper.Map<ComentarioDTO>(comentario);
            return TypedResults.Created($"/comentario/{id}", comentarioDTO);
        }

        static async Task<Results<NoContent, NotFound>> ActualizarComentario(int peliculaId, int id, CrearComentarioDTO crearComentarioDTO, IOutputCacheStore outputCacheStore, IRepositorioPeliculas repositorioPeliculas, IRepositorioComentarios repositorioComentarios, IMapper mapper)
        {
            if (!await repositorioPeliculas.ExistePelicula(peliculaId))
            {
                return TypedResults.NotFound();
            }

            if (!await repositorioComentarios.ExisteComentario(id))
            {
                return TypedResults.NotFound();
            }

            var comentario = mapper.Map<Comentario>(crearComentarioDTO);
            comentario.Id = id;
            comentario.PeliculaId = peliculaId;

            await repositorioComentarios.ActualizarComentario(comentario);
            await outputCacheStore.EvictByTagAsync("comentarios-get", default);

            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> BorrarComentario(int peliculaId, int id, IRepositorioComentarios repositorioComentarios, IOutputCacheStore outputCacheStore)
        {
            if (!await repositorioComentarios.ExisteComentario(id))
            {
                return TypedResults.NotFound();
            }

            await repositorioComentarios.BorrarComentario(id);
            await outputCacheStore.EvictByTagAsync("comentarios-get", default);
            return TypedResults.NoContent();

        }
    }
}