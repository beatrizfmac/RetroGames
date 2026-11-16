using Microsoft.AspNetCore.Mvc;
using RetroGames.Services.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.ApiModels.Jogo;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers;

// Rota /api/Jogo: catálogo paginado, com filtros, e detalhe do jogo.
[ApiController]
[Route("api/[controller]")]
public class JogoController : ControllerBase
{
    private readonly IJogoService _jogoService;

    public JogoController(IJogoService jogoService)
    {
        _jogoService = jogoService;
    }

    // GET /api/Jogo?busca=zelda&idPlataforma=2&idGenero=1&pagina=1&tamanhoPagina=12
    // [FromQuery]: os filtros vêm da query string, não do corpo da requisição.
    [HttpGet]
    public async Task<ActionResult<RetroGamesResponseApiModel<PaginaResponseApiModel<ListarJogoResponseApiModel>>>> Listar(
        [FromQuery] ListarJogoRequestApiModel request)
    {
        var (jogos, total) = await _jogoService.ListarAsync(request.ParaFiltro());

        return Ok(new RetroGamesResponseApiModel<PaginaResponseApiModel<ListarJogoResponseApiModel>>
        {
            ResponseCode = ResponseCodes.SUCESSO,
            Data = jogos.ParaPaginaResponse(total, request),
        });
    }

    // GET /api/Jogo/{id}
    // Se o id não existir, o Service lança JOGO_NAO_ENCONTRADO (404, código 103).
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>>> ListarPorId(int id)
    {
        var jogo = await _jogoService.BuscarPorIdAsync(id);

        return Ok(new RetroGamesResponseApiModel<ListarPorIdJogoResponseApiModel>
        {
            ResponseCode = ResponseCodes.SUCESSO,
            Data = jogo.ParaListarPorIdResponse(),
        });
    }
}
