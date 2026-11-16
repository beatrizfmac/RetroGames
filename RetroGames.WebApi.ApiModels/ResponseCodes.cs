namespace RetroGames.WebApi.ApiModels;

// Catálogo dos códigos que nascem na própria API (Espec. 5.3), de 0 a 99.
// Os erros de negócio (100 em diante) ficam no BusinessErrors, no projeto Services.
public static class ResponseCodes
{
    public const int SUCESSO = 0;
    public const int ERRO = 1;
}
