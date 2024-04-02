using MinimalAPICurso.Entidades;

namespace MinimalAPICurso.Repositorios
{
  public interface IRepositorioGeneros
  {
    Task<int> CrearGenero(Genero genero);
    Task<Genero?> ObtenerGeneroPorId(int id);
    Task<List<Genero>> ObtenerGeneros();
  }
}