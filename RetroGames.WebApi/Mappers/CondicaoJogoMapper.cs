using RetroGames.Model.Model;
using RetroGames.WebApi.ApiModels.CondicaoJogo;

namespace RetroGames.WebApi.Mappers;

// Converte a entidade CondicaoJogo no ApiModel que sai no JSON.
public static class CondicaoJogoMapper
{
    public static ListarCondicaoJogoResponseApiModel ParaResponse(this CondicaoJogo condicaoJogo)
    {
        return new ListarCondicaoJogoResponseApiModel
        {
            Id = condicaoJogo.Id,
            Descricao = condicaoJogo.Descricao,
        };
    }
}
