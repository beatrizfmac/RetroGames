using RetroGames.Model.Model;

namespace RetroGames.Repository.Repositories;

public interface IJogoGeneroRepository
{
    Task<List<JogoGenero>> ListarTodosAsync();
    Task<List<JogoGenero>> ListarPorJogosAsync(IReadOnlyCollection<int> idsJogo);
    Task InserirAsync(int idJogo, IReadOnlyCollection<int> idsGenero);
    Task RemoverPorJogoAsync(int idJogo);
}
