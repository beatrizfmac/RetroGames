using RetroGames.Model.Model;

namespace RetroGames.Services.ServiceModels;

// Um jogo com os dados que vêm de outras tabelas, combinados com LINQ no JogoService.
public class JogoDetalheServiceModel
{
    // Linha de tb_jogo
    public Jogo Jogo { get; set; } = new();

    // no_plataforma de tb_plataforma
    public string NomePlataforma { get; set; } = string.Empty;

    // ds_condicao_jogo de tb_condicao_jogo
    public string DescricaoCondicaoJogo { get; set; } = string.Empty;

    // Gêneros do jogo (tb_jogo_genero + tb_genero)
    public List<Genero> Generos { get; set; } = new();
}
