namespace RetroGames.Services.Erros;

// Lançada pelo Service quando uma regra de negócio é violada.
// Uso: throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);
// A API lê a propriedade Erro para responder com o status e o código certos.
public class BusinessRuleException : Exception
{
    public BusinessRuleException(BusinessError erro)
        : base(erro.DefaultMessage)
    {
        Erro = erro;
    }

    public BusinessError Erro { get; }
}
