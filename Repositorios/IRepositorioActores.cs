using MinimalAPICurso.Entidades;

namespace MinimalAPICurso.Repositorios
{
    public interface IRepositorioActores
    {
        Task ActualizarActor(Actor actor);
        Task BorrarActor(int id);
        Task<int> CrearActor(Actor actor);
        Task<bool> ExisteActor(int id);
        Task<Actor?> ObtenerActorPorId(int id);
        Task<List<Actor>> ObtenerActores();
    }
}