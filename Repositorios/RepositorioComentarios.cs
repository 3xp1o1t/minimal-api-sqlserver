using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using MinimalAPICurso.Entidades;

namespace MinimalAPICurso.Repositorios
{
    public class RepositorioComentarios : IRepositorioComentarios
    {
        private readonly string connectionString;

        public RepositorioComentarios(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<List<Comentario>> ObtenerComentarios()
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var comentarios = await conexion.QueryAsync<Comentario>(@"SP_ObtenerComentarios", commandType: CommandType.StoredProcedure);
                return comentarios.ToList();
            }
        }

        public async Task<Comentario?> ObtenerComentarioPorId(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var comentario = await conexion.QueryFirstOrDefaultAsync<Comentario>(@"SP_ObtenerComentarioPorId", new { id }, commandType: CommandType.StoredProcedure);
                return comentario;
            }
        }

        public async Task<int> CrearComentario(Comentario comentario)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var id = await conexion.QuerySingleAsync<int>(@"SP_CrearComentario", new { comentario.Cuerpo, comentario.PeliculaId }, commandType: CommandType.StoredProcedure);
                comentario.Id = id;
                return id;
            }
        }

        public async Task<bool> ExisteComentario(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var existe = await conexion.QuerySingleAsync<bool>(@"SP_ExisteComentarioPorId", new { id }, commandType: CommandType.StoredProcedure);
                return existe;
            }
        }

        public async Task ActualizarComentario(Comentario comentario)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync(@"SP_ActualizarComentario", comentario, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task BorrarComentario(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync(@"SP_BorrarComentario", new { id }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}