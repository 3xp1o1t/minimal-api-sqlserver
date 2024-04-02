using System;
using System.Collections.Generic;
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
                var id = await conexion.QuerySingleAsync<int>(@"
                INSERT INTO Generos (Nombre) VALUES (@Nombre);
                SELECT SCOPE_IDENTITY();", genero);

                genero.Id = id;

                return id;
            }

        }

        public async Task<List<Genero>> ObtenerGeneros()
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var generos = await conexion.QueryAsync<Genero>(@"SELECT * FROM Generos");

                return generos.ToList();
            }
        }

        public async Task<Genero?> ObtenerGeneroPorId(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var genero = await conexion.QueryFirstOrDefaultAsync<Genero>(@"SELECT * FROM Generos WHERE Id = @Id", new { id });

                return genero;
            }
        }
    }


}