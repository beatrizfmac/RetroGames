using RetroGames.Model.Model;

namespace RetroGames.Repository.Repositories;

public interface ICondicaoJogoRepository
{
    Task<List<CondicaoJogo>> ListarAsync();
}
