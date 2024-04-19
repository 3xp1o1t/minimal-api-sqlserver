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
    }
}