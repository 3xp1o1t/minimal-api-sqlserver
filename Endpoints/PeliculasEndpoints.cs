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
    public static class PeliculasEndpoints
    {
        private static readonly string contenedor = "peliculas";
        public static RouteGroupBuilder MapPeliculas(this RouteGroupBuilder group)
        {
            group.MapGet("/", ObtenerPeliculas).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("peliculas-get"));
            group.MapGet("/{id:int}", ObtenerPeliculaPorId);
            group.MapPost("/", CrearPelicula).DisableAntiforgery();
            group.MapPut("/{id:int}", ActualizarPelicula).DisableAntiforgery();
            group.MapDelete("/{id:int}", BorrarPelicula);
            group.MapPost("/{id:int}/asignargeneros", AsignarGeneros);
            return group;
        }

        static async Task<Ok<List<PeliculaDTO>>> ObtenerPeliculas(IRepositorioPeliculas repositorioPeliculas, IMapper mapper, int pagina = 1, int registrosPorPagina = 10)
        {
            var paginacion = new PaginacionDTO { Pagina = pagina, RegistrosPorPagina = registrosPorPagina };
            var peliculas = await repositorioPeliculas.ObtenerPeliculas(paginacion);
            var peliculasDTO = mapper.Map<List<PeliculaDTO>>(peliculas);
            return TypedResults.Ok(peliculasDTO);
        }

        static async Task<Results<Ok<PeliculaDTO>, NotFound>> ObtenerPeliculaPorId(int id, IRepositorioPeliculas repositorioPeliculas, IMapper mapper)
        {
            var pelicula = await repositorioPeliculas.ObtenerPeliculaPorId(id);
            if (pelicula is null)
            {
                return TypedResults.NotFound();
            }

            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);
            return TypedResults.Ok(peliculaDTO);
        }

        static async Task<Created<PeliculaDTO>> CrearPelicula([FromForm] CrearPeliculaDTO crearPeliculaDTO, IRepositorioPeliculas repositorioPeliculas, IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var pelicula = mapper.Map<Pelicula>(crearPeliculaDTO);

            if (crearPeliculaDTO.Poster is not null)
            {
                var url = await almacenadorArchivos.AlmacenarArchivo(contenedor, crearPeliculaDTO.Poster);
                pelicula.Poster = url;
            }

            var id = await repositorioPeliculas.CrearPelicula(pelicula);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);
            var peliculaDTO = mapper.Map<PeliculaDTO>(pelicula);
            return TypedResults.Created($"/peliculas/{id}", peliculaDTO);
        }

        static async Task<Results<NoContent, NotFound>> ActualizarPelicula(int id, [FromForm] CrearPeliculaDTO crearPeliculaDTO, IRepositorioPeliculas repositorioPeliculas, IAlmacenadorArchivos almacenadorArchivos, IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var peliculaEnDB = await repositorioPeliculas.ObtenerPeliculaPorId(id);

            if (peliculaEnDB is null)
            {
                return TypedResults.NotFound();
            }

            var peliculaParaActualizar = mapper.Map<Pelicula>(crearPeliculaDTO);
            peliculaParaActualizar.Id = id;
            peliculaParaActualizar.Poster = peliculaEnDB.Poster;

            if (crearPeliculaDTO.Poster is not null)
            {
                var url = await almacenadorArchivos.EditarArchivo(peliculaParaActualizar.Poster, contenedor, crearPeliculaDTO.Poster);
                peliculaParaActualizar.Poster = url;
            }

            await repositorioPeliculas.ActualizarPelicula(peliculaParaActualizar);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound>> BorrarPelicula(int id, IRepositorioPeliculas repositorioPeliculas, IOutputCacheStore outputCacheStore, IAlmacenadorArchivos almacenadorArchivos)
        {
            var peliculaEnDB = await repositorioPeliculas.ObtenerPeliculaPorId(id);

            if (peliculaEnDB is null)
            {
                return TypedResults.NotFound();
            }

            await repositorioPeliculas.BorrarPelicula(id);
            await almacenadorArchivos.BorrarArchivo(peliculaEnDB.Poster, contenedor);
            await outputCacheStore.EvictByTagAsync("peliculas-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NoContent, NotFound, BadRequest<string>>> AsignarGeneros(int id, List<int> generosIds, IRepositorioPeliculas repositorioPeliculas, IRepositorioGeneros repositorioGeneros)
        {
            if (!await repositorioPeliculas.ExistePelicula(id))
            {
                return TypedResults.NotFound();
            }

            var generosExistentes = new List<int>();
            if (generosIds.Count != 0)
            {
                generosExistentes = await repositorioGeneros.ExistenGeneros(generosIds);
            }

            if (generosExistentes.Count != generosIds.Count)
            {
                var generosNoExistentes = generosIds.Except(generosExistentes);

                return TypedResults.BadRequest($"Los generos de id {string.Join(",", generosNoExistentes)} no existen.");
            }

            await repositorioPeliculas.AsignarGeneros(id, generosIds);
            return TypedResults.NoContent();
        }
    }
}