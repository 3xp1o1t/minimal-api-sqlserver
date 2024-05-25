using MinimalAPICurso.Entidades;

namespace MinimalAPICurso.Repositorios
{
  public interface IRepositorioGeneros
  {
    Task<int> CrearGenero(Genero genero);
    Task<Genero?> ObtenerGeneroPorId(int id);
    Task<List<Genero>> ObtenerGeneros();
    Task<bool> ExisteGenero(int id);
    Task<bool> ExisteGenero(int id, string nombre);
    Task ActualizarGenero(Genero genero);
    Task BorrarGenero(int id);
    Task<List<int>> ExistenGeneros(List<int> ids);
  }
}