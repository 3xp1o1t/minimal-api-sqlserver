using MinimalAPICurso.Entidades;

namespace MinimalAPICurso.Repositorios
{
    public interface IRepositorioComentarios
    {
        Task ActualizarComentario(Comentario comentario);
        Task BorrarComentario(int id);
        Task<int> CrearComentario(Comentario comentario);
        Task<bool> ExisteComentario(int id);
        Task<Comentario?> ObtenerComentarioPorId(int id);
        Task<List<Comentario>> ObtenerComentarios();
    }
}