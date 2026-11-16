namespace RetroGames.WebApi.ApiModels.Jogo;

// Parâmetros da listagem, lidos da query string:
// /api/Jogo?busca=zelda&idPlataforma=2&idGenero=1&pagina=1&tamanhoPagina=12
public class ListarJogoRequestApiModel
{
    public string? Busca { get; set; }
    public int? IdPlataforma { get; set; }
    public int? IdGenero { get; set; }
    public int Pagina { get; set; } = 1;
    public int TamanhoPagina { get; set; } = 12;
}
