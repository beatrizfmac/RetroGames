using Microsoft.AspNetCore.Mvc;
using RetroGames.Services.Services;
using RetroGames.WebApi.ApiModels;
using RetroGames.WebApi.ApiModels.Plataforma;
using RetroGames.WebApi.Mappers;

namespace RetroGames.WebApi.Controllers;

// Rota /api/Plataforma (vem do nome da classe, sem o sufixo Controller).
// Sem regra, SQL nem try/catch: chama o Service, converte e devolve o envelope.
[ApiController]
[Route("api/[controller]")]
public class PlataformaController : ControllerBase
{
    private readonly IPlataformaService _plataformaService;

    public PlataformaController(IPlataformaService plataformaService)
    {
        _plataformaService = plataformaService;
    }

    // GET /api/Plataforma
    [HttpGet]
    public async Task<ActionResult<RetroGamesResponseApiModel<List<ListarPlataformaResponseApiModel>>>> Listar()
    {
        var plataformas = await _plataformaService.ListarAsync();

        return Ok(new RetroGamesResponseApiModel<List<ListarPlataformaResponseApiModel>>
        {
            ResponseCode = ResponseCodes.SUCESSO,
            Data = plataformas.Select(plataforma => plataforma.ParaResponse()).ToList(),
        });
    }

    // GET /api/Plataforma/{id}
    // Se o id não existir, o Service lança BusinessRuleException (tratada na Etapa 3).
    [HttpGet("{id:int}")]
    public async Task<ActionResult<RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>>> ListarPorId(int id)
    {
        var plataforma = await _plataformaService.BuscarPorIdAsync(id);

        return Ok(new RetroGamesResponseApiModel<ListarPlataformaResponseApiModel>
        {
            ResponseCode = ResponseCodes.SUCESSO,
            Data = plataforma.ParaResponse(),
        });
    }
}
