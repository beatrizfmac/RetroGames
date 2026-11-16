namespace RetroGames.WebApi.ApiModels.Jogo;

// Um jogo na listagem do catálogo.
public class ListarJogoResponseApiModel
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int AnoLancamento { get; set; }
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public bool Ativo { get; set; }
    public string NomePlataforma { get; set; } = string.Empty;
    public string DescricaoCondicaoJogo { get; set; } = string.Empty;

    // Só os nomes dos gêneros.
    public List<string> Generos { get; set; } = new();
}
