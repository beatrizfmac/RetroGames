using RetroGames.Services.ServiceModels;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.ApiModels.Jogo;

namespace RetroGames.WebApi.Mappers;

// Converte o request no filtro do Service, e o JogoDetalheServiceModel nos ApiModels de saída.
public static class JogoMapper
{
    public static ListarJogoFiltroServiceModel ParaFiltro(this ListarJogoRequestApiModel request)
    {
        return new ListarJogoFiltroServiceModel
        {
            Busca = request.Busca,
            IdPlataforma = request.IdPlataforma,
            IdGenero = request.IdGenero,
            Pagina = request.Pagina,
            TamanhoPagina = request.TamanhoPagina,
        };
    }

    public static ListarJogoResponseApiModel ParaListarResponse(this JogoDetalheServiceModel detalhe)
    {
        return new ListarJogoResponseApiModel
        {
            Id = detalhe.Jogo.Id,
            Nome = detalhe.Jogo.Nome,
            AnoLancamento = detalhe.Jogo.AnoLancamento,
            Preco = detalhe.Jogo.Preco,
            Estoque = detalhe.Jogo.Estoque,
            Ativo = detalhe.Jogo.Ativo,
            NomePlataforma = detalhe.NomePlataforma,
            DescricaoCondicaoJogo = detalhe.DescricaoCondicaoJogo,
            Generos = detalhe.Generos.Select(genero => genero.Nome).ToList(),
        };
    }

    public static ListarPorIdJogoResponseApiModel ParaListarPorIdResponse(this JogoDetalheServiceModel detalhe)
    {
        return new ListarPorIdJogoResponseApiModel
        {
            Id = detalhe.Jogo.Id,
            Nome = detalhe.Jogo.Nome,
            Descricao = detalhe.Jogo.Descricao,
            AnoLancamento = detalhe.Jogo.AnoLancamento,
            Preco = detalhe.Jogo.Preco,
            Estoque = detalhe.Jogo.Estoque,
            Ativo = detalhe.Jogo.Ativo,
            IdPlataforma = detalhe.Jogo.IdPlataforma,
            NomePlataforma = detalhe.NomePlataforma,
            IdCondicaoJogo = detalhe.Jogo.IdCondicaoJogo,
            DescricaoCondicaoJogo = detalhe.DescricaoCondicaoJogo,
            Generos = detalhe.Generos.Select(genero => genero.ParaResponse()).ToList(),
        };
    }

    // Monta a página com os totais. O Service já corrige página e tamanho menores
    // que 1; aqui é feita a mesma correção para os números que voltam na resposta.
    public static PaginaResponseApiModel<ListarJogoResponseApiModel> ParaPaginaResponse(
        this List<JogoDetalheServiceModel> jogos, int total, ListarJogoRequestApiModel request)
    {
        var pagina = Math.Max(request.Pagina, 1);
        var tamanhoPagina = Math.Max(request.TamanhoPagina, 1);

        return new PaginaResponseApiModel<ListarJogoResponseApiModel>
        {
            Itens = jogos.Select(jogo => jogo.ParaListarResponse()).ToList(),
            Pagina = pagina,
            TamanhoPagina = tamanhoPagina,
            TotalItens = total,
            TotalPaginas = (int)Math.Ceiling(total / (double)tamanhoPagina),
        };
    }
}
