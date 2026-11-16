using RetroGames.Model.Model;
using RetroGames.Repository;

namespace RetroGames.Services.Services;

// Gêneros só são lidos e não têm regra de negócio. O Service existe para que o
// controller fale só com Services, nunca direto com o Repository.
public class GeneroService : IGeneroService
{
    private readonly IRepositorySet _repositorio;

    public GeneroService(IRepositorySet repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<List<Genero>> ListarAsync()
    {
        return await _repositorio.Genero.ListarAsync();
    }
}
