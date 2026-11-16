namespace RetroGames.WebApi.ApiModels;

// Envelope de todas as respostas: { "responseCode", "data", "errorMessage" }.
// Esta versão, sem Data, é usada nas respostas de erro (a Espec. omite data no erro).
public class RetroGamesResponseApiModel
{
    public int ResponseCode { get; set; }
    public string? ErrorMessage { get; set; }
}

// Versão com Data, usada nas respostas de sucesso.
public class RetroGamesResponseApiModel<T> : RetroGamesResponseApiModel
{
    public T? Data { get; set; }
}
