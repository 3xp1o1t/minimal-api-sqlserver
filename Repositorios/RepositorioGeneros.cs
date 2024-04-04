using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using MinimalAPICurso.Entidades;

namespace MinimalAPICurso.Repositorios
{
    public class RepositorioGeneros : IRepositorioGeneros
    {
        private readonly string? connectionString;

        public RepositorioGeneros(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> CrearGenero(Genero genero)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var id = await conexion.QuerySingleAsync<int>(@"SP_CrearGenero", new { genero.Nombre }, commandType: CommandType.StoredProcedure);

                genero.Id = id;

                return id;
            }

        }

        public async Task<List<Genero>> ObtenerGeneros()
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var generos = await conexion.QueryAsync<Genero>(@"SP_ObtenerGeneros", commandType: CommandType.StoredProcedure);

                return generos.ToList();
            }
        }

        public async Task<Genero?> ObtenerGeneroPorId(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var genero = await conexion.QueryFirstOrDefaultAsync<Genero>(@"SP_ObtenerGeneroPorId", new { id }, commandType: CommandType.StoredProcedure);

                return genero;
            }
        }

        public async Task<bool> ExisteGenero(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var existe = await conexion.QuerySingleAsync<bool>(@"SP_ExisteGeneroPorId", new { id }, commandType: CommandType.StoredProcedure);

                return existe;
            }
        }

        public async Task ActualizarGenero(Genero genero)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync(@"SP_ActualizarGenero", genero, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task BorrarGenero(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync(@"SP_BorrarGenero", new { id }, commandType: CommandType.StoredProcedure);
            }
        }
    }


}