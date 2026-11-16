namespace RetroGames.WebApi.ApiModels;

// Uma página de resultados de qualquer listagem paginada.
public class PaginaResponseApiModel<T>
{
    public List<T> Itens { get; set; } = new();
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
    public int TotalItens { get; set; }
    public int TotalPaginas { get; set; }
}
