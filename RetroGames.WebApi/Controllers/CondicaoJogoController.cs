using Microsoft.AspNetCore.Mvc;
using RetroGames.Services.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.ApiModels.CondicaoJogo;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers;

// Rota /api/CondicaoJogo: vem do nome da classe pelo [controller], sem texto fixo.
[ApiController]
[Route("api/[controller]")]
public class CondicaoJogoController : ControllerBase
{
    private readonly ICondicaoJogoService _condicaoJogoService;

    public CondicaoJogoController(ICondicaoJogoService condicaoJogoService)
    {
        _condicaoJogoService = condicaoJogoService;
    }

    // GET /api/CondicaoJogo
    [HttpGet]
    public async Task<ActionResult<RetroGamesResponseApiModel<List<ListarCondicaoJogoResponseApiModel>>>> Listar()
    {
        var condicoes = await _condicaoJogoService.ListarAsync();

        return Ok(new RetroGamesResponseApiModel<List<ListarCondicaoJogoResponseApiModel>>
        {
            ResponseCode = ResponseCodes.SUCESSO,
            Data = condicoes.Select(condicao => condicao.ParaResponse()).ToList(),
        });
    }
}
