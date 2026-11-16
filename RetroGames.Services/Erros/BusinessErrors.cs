using System.Net;

namespace RetroGames.Services.Erros;

// Catálogo dos erros de negócio (Espec. 5.3). Cada erro é definido uma única vez,
// aqui. Um erro novo recebe o próximo código livre.
public static class BusinessErrors
{
    // RN05: plataforma pedida, ou plataforma do jogo, não existe.
    public static readonly BusinessError PLATAFORMA_NAO_ENCONTRADA =
        new(100, "Plataforma não encontrada.", HttpStatusCode.NotFound);

    // RN01: já existe outra plataforma com o mesmo nome.
    public static readonly BusinessError PLATAFORMA_NOME_DUPLICADO =
        new(101, "Já existe uma plataforma com este nome.", HttpStatusCode.Conflict);

    // RN02: plataforma com jogos vinculados não pode ser excluída.
    public static readonly BusinessError PLATAFORMA_POSSUI_JOGOS =
        new(102, "Não é possível excluir: existem jogos desta plataforma.", HttpStatusCode.Conflict);

    // O id do jogo não existe.
    public static readonly BusinessError JOGO_NAO_ENCONTRADO =
        new(103, "Jogo não encontrado.", HttpStatusCode.NotFound);

    // RN05: condição do jogo não existe.
    public static readonly BusinessError CONDICAO_NAO_ENCONTRADA =
        new(104, "Condição de jogo não encontrada.", HttpStatusCode.NotFound);

    // RN05: gênero do jogo não existe.
    public static readonly BusinessError GENERO_NAO_ENCONTRADO =
        new(105, "Gênero não encontrado.", HttpStatusCode.NotFound);
}
