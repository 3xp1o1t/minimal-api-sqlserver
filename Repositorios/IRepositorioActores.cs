using MinimalAPICurso.DTOs;
using MinimalAPICurso.Entidades;

namespace MinimalAPICurso.Repositorios
{
    public interface IRepositorioActores
    {
        Task ActualizarActor(Actor actor);
        Task BorrarActor(int id);
        Task<int> CrearActor(Actor actor);
        Task<bool> ExisteActor(int id);
        Task<List<int>> ExistenActores(List<int> ids);
        Task<Actor?> ObtenerActorPorId(int id);
        Task<List<Actor>> ObtenerActores(PaginacionDTO paginacionDTO);
        Task<List<Actor>> ObtenerActorPorNombre(string nombre);
    }
}