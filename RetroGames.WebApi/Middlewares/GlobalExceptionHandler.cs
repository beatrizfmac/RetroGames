using Microsoft.AspNetCore.Diagnostics;
using RetroGames.Services.Erros;
using RetroGames.WebApi.ApiModels;

namespace RetroGames.WebApi.Middlewares;

// Único ponto que transforma exceções em resposta HTTP, sempre no formato do envelope.
// Por isso nenhum controller precisa de try/catch.
public class GlobalExceptionHandler : IExceptionHandler
{
    private const string MensagemErroInesperado = "Ocorreu um erro inesperado. Tente novamente mais tarde.";

    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        int status;
        RetroGamesResponseApiModel resposta;

        if (exception is BusinessRuleException excecaoNegocio)
        {
            // Regra violada: status (400, 404 ou 409) e código vêm do erro do catálogo.
            status = (int)excecaoNegocio.Erro.Status;
            resposta = new RetroGamesResponseApiModel
            {
                ResponseCode = excecaoNegocio.Erro.Code,
                ErrorMessage = excecaoNegocio.Message,
            };
        }
        else
        {
            // Erro inesperado: os detalhes vão só para o log (terminal da API),
            // nunca para a resposta (nada de SQL, nome de tabela ou caminho de arquivo).
            _logger.LogError(exception, "Erro não tratado em {Metodo} {Caminho}",
                httpContext.Request.Method, httpContext.Request.Path);

            status = StatusCodes.Status500InternalServerError;
            resposta = new RetroGamesResponseApiModel
            {
                ResponseCode = ResponseCodes.ERRO,
                ErrorMessage = MensagemErroInesperado,
            };
        }

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(resposta, cancellationToken);

        // true: a exceção foi tratada aqui.
        return true;
    }
}
