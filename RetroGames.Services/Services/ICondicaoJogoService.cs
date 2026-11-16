using RetroGames.Model.Model;

namespace RetroGames.Services.Services;

public interface ICondicaoJogoService
{
    Task<List<CondicaoJogo>> ListarAsync();
}
