using RetroGames.Model.Model;

namespace RetroGames.Services.Services;

public interface IGeneroService
{
    Task<List<Genero>> ListarAsync();
}
