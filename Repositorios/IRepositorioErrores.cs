using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalAPICurso.Entidades;

namespace MinimalAPICurso.Repositorios
{
    public interface IRepositorioErrores
    {
        Task CrearMensajeError(Error error);
    }
}