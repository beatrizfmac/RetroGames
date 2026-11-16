using RetroGames.Model.Model;
using RetroGames.WebApi.ApiModels.Genero;

namespace RetroGames.WebApi.Mappers;

// Converte a entidade Genero no ApiModel que sai no JSON.
public static class GeneroMapper
{
    public static ListarGeneroResponseApiModel ParaResponse(this Genero genero)
    {
        return new ListarGeneroResponseApiModel
        {
            Id = genero.Id,
            Nome = genero.Nome,
        };
    }
}
