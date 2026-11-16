using Microsoft.AspNetCore.Mvc;
using RetroGames.Services.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.ApiModels.Genero;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers;

// Rota /api/Genero. Lista de apoio para os filtros do catálogo e os formulários.
[ApiController]
[Route("api/[controller]")]
public class GeneroController : ControllerBase
{
    private readonly IGeneroService _generoService;

    public GeneroController(IGeneroService generoService)
    {
        _generoService = generoService;
    }

    // GET /api/Genero
    [HttpGet]
    public async Task<ActionResult<RetroGamesResponseApiModel<List<ListarGeneroResponseApiModel>>>> Listar()
    {
        var generos = await _generoService.ListarAsync();

        return Ok(new RetroGamesResponseApiModel<List<ListarGeneroResponseApiModel>>
        {
            ResponseCode = ResponseCodes.SUCESSO,
            Data = generos.Select(genero => genero.ParaResponse()).ToList(),
        });
    }
}
