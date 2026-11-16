using RetroGames.WebApi.ApiModels.Genero;

namespace RetroGames.WebApi.ApiModels.Jogo;

// O detalhe de um jogo: os dados da listagem, mais a descrição, os ids de
// plataforma e condição (úteis para o formulário de edição) e os gêneros com id e nome.
public class ListarPorIdJogoResponseApiModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public int AnoLancamento { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public bool Ativo { get; set; }
    public int IdPlataforma { get; set; }
    public string NomePlataforma { get; set; } = string.Empty;
    public int IdCondicaoJogo { get; set; }
    public string DescricaoCondicaoJogo { get; set; } = string.Empty;
    public List<ListarGeneroResponseApiModel> Generos { get; set; } = new();
}
