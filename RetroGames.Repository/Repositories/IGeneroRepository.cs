using RetroGames.Model.Model;

namespace RetroGames.Repository.Repositories;

public interface IGeneroRepository
{
    Task<List<Genero>> ListarAsync();
}
