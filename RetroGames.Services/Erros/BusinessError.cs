using System.Net;

namespace RetroGames.Services.Erros;

// Um erro de negócio do catálogo: código (100 em diante), mensagem padrão e o
// status HTTP com que a API vai responder. O Service não usa HTTP: o status é só
// um valor guardado aqui, lido pelo GlobalExceptionHandler da API.
public record BusinessError(int Code, string DefaultMessage, HttpStatusCode Status);
