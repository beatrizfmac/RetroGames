using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services.Services;

// Condições de jogo só são lidas e não têm regra de negócio. O Service existe para
// que o controller fale só com Services, nunca direto com o Repository.
public class CondicaoJogoService : ICondicaoJogoService
{
    private readonly IRepositorySet _repositorio;

    public CondicaoJogoService(IRepositorySet repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<List<CondicaoJogo>> ListarAsync()
    {
        return await _repositorio.CondicaoJogo.ListarAsync();
    }
}
