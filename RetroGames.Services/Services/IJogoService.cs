using RetroGames.Model.Model;
using RetroGames.Services.ServiceModels;

namespace RetroGames.Services.Services;

public interface IJogoService
{
    // Devolve a página pedida (já com o detalhe de cada jogo) e o total encontrado.
    Task<(List<JogoDetalheServiceModel> Jogos, int Total)> ListarAsync(ListarJogoFiltroServiceModel filtro);

    Task<JogoDetalheServiceModel> BuscarPorIdAsync(int id);
    Task<JogoDetalheServiceModel> InserirAsync(Jogo jogo, IReadOnlyCollection<int> idsGenero);
    Task<JogoDetalheServiceModel> AlterarAsync(Jogo jogo, IReadOnlyCollection<int> idsGenero);
    Task DesativarAsync(int id);
}
