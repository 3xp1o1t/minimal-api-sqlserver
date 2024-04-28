using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using MinimalAPICurso.DTOs;
using MinimalAPICurso.Endpoints;
using MinimalAPICurso.Entidades;

namespace MinimalAPICurso.Repositorios
{
    public class RepositorioPeliculas : IRepositorioPeliculas
    {
        private readonly string connectionString;
        private readonly HttpContext httpContext;

        public RepositorioPeliculas(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
            httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<List<Pelicula>> ObtenerPeliculas(PaginacionDTO paginacionDTO)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var peliculas = await conexion.QueryAsync<Pelicula>(@"SP_ObtenerPeliculas", new { paginacionDTO.Pagina, paginacionDTO.RegistrosPorPagina }, commandType: CommandType.StoredProcedure);

                var cantidadPeliculas = await conexion.QuerySingleAsync<int>(@"SP_CantidadPeliculas", commandType: CommandType.StoredProcedure);

                httpContext.Response.Headers.Append("cantidadTotalRegistros", cantidadPeliculas.ToString());

                return peliculas.ToList();
            }
        }

        public async Task<Pelicula?> ObtenerPeliculaPorId(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                using (var multi = await conexion.QueryMultipleAsync(@"SP_ObtenerPeliculaPorId", new { id }, commandType: CommandType.StoredProcedure))
                {
                    var pelicula = await multi.ReadFirstAsync<Pelicula>();
                    var comentarios = await multi.ReadAsync<Comentario>();
                    var generos = await multi.ReadAsync<Genero>();
                    var actores = await multi.ReadAsync<ActorPeliculaDTO>();

                    foreach (var genero in generos)
                    {
                        pelicula.GenerosPeliculas.Add(new GeneroPelicula
                        {
                            GeneroId = genero.Id,
                            Genero = genero
                        });
                    }

                    foreach (var actor in actores)
                    {
                        pelicula.ActoresPeliculas.Add(new ActorPelicula
                        {
                            ActorId = actor.Id,
                            Personaje = actor.Personaje,
                            Actor = new Actor { Nombre = actor.Nombre }
                        });
                    }

                    pelicula.Comentarios = comentarios.ToList();
                    return pelicula;
                }

            }
        }

        public async Task<int> CrearPelicula(Pelicula pelicula)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var id = await conexion.QuerySingleAsync<int>(@"SP_CrearPelicula", new { pelicula.Titulo, pelicula.EnCines, pelicula.FechaLanzamiento, pelicula.Poster }, commandType: CommandType.StoredProcedure);

                pelicula.Id = id;

                return id;
            }
        }

        public async Task<bool> ExistePelicula(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var existe = await conexion.QuerySingleAsync<bool>(@"SP_ExistePeliculaPorId", new { id }, commandType: CommandType.StoredProcedure);

                return existe;
            }
        }

        public async Task ActualizarPelicula(Pelicula pelicula)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync(@"SP_ActualizarPelicula", pelicula, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task BorrarPelicula(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync(@"SP_BorrarPelicula", new { id });
            }
        }

        public async Task AsignarGeneros(int id, List<int> generosIds)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));

            foreach (var generoId in generosIds)
            {
                dt.Rows.Add(generoId);
            }

            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync(@"SP_Peliculas_AsignarGeneros", new { peliculaId = id, generosIds = dt }, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task AsignarActores(int id, List<ActorPelicula> actores)
        {
            for (int i = 1; i <= actores.Count; i++)
            {
                actores[i - 1].Orden = i;
            }

            var dt = new DataTable();
            dt.Columns.Add("ActorId", typeof(int));
            dt.Columns.Add("Persona", typeof(string));
            dt.Columns.Add("Orden", typeof(int));

            foreach (var actorPelicula in actores)
            {
                dt.Rows.Add(actorPelicula.ActorId, actorPelicula.Personaje, actorPelicula.Orden);
            }

            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync(@"SP_Peliculas_AsignarActores",
                new { peliculaId = id, actores = dt }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}