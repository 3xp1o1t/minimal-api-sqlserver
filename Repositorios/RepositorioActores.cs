using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using MinimalAPICurso.DTOs;
using MinimalAPICurso.Entidades;

namespace MinimalAPICurso.Repositorios
{
    public class RepositorioActores : IRepositorioActores
    {
        private readonly string connectionString;
        private readonly HttpContext httpContext;

        public RepositorioActores(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
            httpContext = httpContextAccessor.HttpContext!;
        }

        public async Task<List<Actor>> ObtenerActores(PaginacionDTO paginacionDTO)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var actores = await conexion.QueryAsync<Actor>(@"SP_ObtenerActores",
                new { paginacionDTO.Pagina, paginacionDTO.RegistrosPorPagina }, commandType: CommandType.StoredProcedure);

                var cantidadActores = await conexion.QuerySingleAsync<int>(@"SP_CantidadActores", commandType: CommandType.StoredProcedure);

                httpContext.Response.Headers.Append("cantidadTotalRegistros", cantidadActores.ToString());
                return actores.ToList();
            }
        }

        public async Task<Actor?> ObtenerActorPorId(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var actor = await conexion.QueryFirstOrDefaultAsync<Actor>(@"SP_ObtenerActorPorId", new { id }, commandType: CommandType.StoredProcedure);
                return actor;
            }
        }

        public async Task<List<Actor>> ObtenerActorPorNombre(string nombre)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var actores = await conexion.QueryAsync<Actor>(@"SP_ObtenerActorPorNombre", new { nombre }, commandType: CommandType.StoredProcedure);
                return actores.ToList();
            }
        }

        public async Task<int> CrearActor(Actor actor)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var id = await conexion.QuerySingleAsync<int>(@"SP_CrearActor", new { actor.Nombre, actor.FechaNacimiento, actor.Foto }, commandType: CommandType.StoredProcedure);
                actor.Id = id;
                return id;
            }
        }

        public async Task ActualizarActor(Actor actor)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync(@"SP_ActualizarActor", actor, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<bool> ExisteActor(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var existe = await conexion.QuerySingleAsync<bool>(@"SP_ExisteActorPorId", new { id }, commandType: CommandType.StoredProcedure);

                return existe;
            }
        }

        public async Task BorrarActor(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync(@"SP_BorrarActor", new { id });
            }
        }
    }
}