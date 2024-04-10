using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalAPICurso.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int registrosPorPagina = 10;
        private readonly int cantidadMaximaRegistrosPorPagina = 50;

        public int RegistrosPorPagina
        {
            get { return registrosPorPagina; }
            set
            {
                registrosPorPagina = (value > cantidadMaximaRegistrosPorPagina) ? cantidadMaximaRegistrosPorPagina : value;
            }
        }
    }
}