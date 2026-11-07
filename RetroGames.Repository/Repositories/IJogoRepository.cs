using RetroGames.Model.Model;

namespace RetroGames.Repository.Repositories;

public interface IJogoRepository
{
    // idsJogo: filtro por lista de ids, usado pelo Service no filtro por gênero.
    // null = sem filtro; lista vazia = nenhum jogo.
    Task<(List<Jogo> Jogos, int Total)> ListarAsync(
        string? busca,
        int? idPlataforma,
        IReadOnlyCollection<int>? idsJogo,
        bool incluirInativos,
        int pagina,
        int tamanhoPagina);

    Task<Jogo?> BuscarPorIdAsync(int id);
    Task<int> InserirAsync(Jogo jogo);
    Task AlterarAsync(Jogo jogo);
    Task DesativarAsync(int id);
}
