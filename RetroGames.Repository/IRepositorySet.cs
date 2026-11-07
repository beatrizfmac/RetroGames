using RetroGames.Repository.Repositories;

namespace RetroGames.Repository;

// Reúne todos os repositórios num só objeto. O Service recebe este conjunto e usa
// repositorio.Plataforma, repositorio.Jogo, repositorio.JogoGenero etc.
public interface IRepositorySet
{
    IPlataformaRepository Plataforma { get; }
    IJogoRepository Jogo { get; }
    IJogoGeneroRepository JogoGenero { get; }
    IGeneroRepository Genero { get; }
    ICondicaoJogoRepository CondicaoJogo { get; }
}
