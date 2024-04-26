using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalAPICurso.DTOs
{
    public class AsignarActorPeliculaDTO
    {
        public int ActorId { get; set; }
        public string Personaje { get; set; } = null!;
    }
}