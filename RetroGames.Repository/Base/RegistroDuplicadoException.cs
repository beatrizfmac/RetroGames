namespace RetroGames.Repository.Base;

// Lançada quando o MySQL recusa um valor repetido numa coluna UNIQUE (erro 1062).
// O Service captura esta exceção e a converte no erro de negócio correspondente.
public class RegistroDuplicadoException : Exception
{
    public RegistroDuplicadoException(string mensagem, Exception excecaoOriginal)
        : base(mensagem, excecaoOriginal)
    {
    }
}
