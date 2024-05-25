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
    public class RepositorioErrores : IRepositorioErrores
    {
        private readonly string connectionString;

        public RepositorioErrores(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task CrearMensajeError(Error error)
        {
            using (var conexion = new SqlConnection(connectionString))
            {
                await conexion.ExecuteAsync(@"SP_CrearMensajeError",
                new { error.MensajeDeError, error.StackTrace, error.Fecha },
                commandType: CommandType.StoredProcedure);
            }
        }

    }
}