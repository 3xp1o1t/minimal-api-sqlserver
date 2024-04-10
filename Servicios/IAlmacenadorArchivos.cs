using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalAPICurso.Servicios
{
    public interface IAlmacenadorArchivos
    {
        // contenedor = folder
        Task BorrarArchivo(string? ruta, string contenedor);
        Task<string> AlmacenarArchivo(string contenedor, IFormFile archivo);
        async Task<string> EditarArchivo(string? ruta, string contenedor, IFormFile archivo)
        {
            await BorrarArchivo(ruta, contenedor);
            return await AlmacenarArchivo(contenedor, archivo);
        }
    }
}