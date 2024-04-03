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
                var generos = await conexion.QueryAsync<Genero>(@"SELECT * FROM Generos ORDER BY Nombre");

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

        public async Task<bool> ExisteGenero(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                var existe = await conexion.QuerySingleAsync<bool>(@"
                IF EXISTS (SELECT 1 FROM Generos WHERE Id = @Id)
                    SELECT 1
                ELSE
                    SELECT 0", new { id });

                return existe;
            }
        }

        public async Task ActualizarGenero(Genero genero)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync(@"UPDATE Generos
                SET Nombre = @Nombre WHERE Id = @Id", genero);
            }
        }

        public async Task BorrarGenero(int id)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync(@"DELETE FROM Generos WHERE Id = @Id", new { id });
            }
        }
    }


}