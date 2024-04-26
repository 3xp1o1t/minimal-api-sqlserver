using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MinimalAPICurso.DTOs;
using MinimalAPICurso.Entidades;

namespace MinimalAPICurso.Repositorios
{
    public interface IRepositorioPeliculas
    {
        Task ActualizarPelicula(Pelicula pelicula);
        Task BorrarPelicula(int id);
        Task<int> CrearPelicula(Pelicula pelicula);
        Task<bool> ExistePelicula(int id);
        Task<Pelicula?> ObtenerPeliculaPorId(int id);
        Task<List<Pelicula>> ObtenerPeliculas(PaginacionDTO paginacionDTO);
        Task AsignarGeneros(int id, List<int> generosIds);
    }
}