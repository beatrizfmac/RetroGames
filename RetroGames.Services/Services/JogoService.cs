using System.Transactions;
using RetroGames.Model.Model;
using RetroGames.Repository;
using RetroGames.Services.Erros;
using RetroGames.Services.ServiceModels;

namespace RetroGames.Services.Services;

// Regras de negócio de jogos (RN03 a RN05). O Repository devolve jogos só com os ids
// de plataforma e condição; aqui os dados das outras tabelas são combinados com LINQ.
public class JogoService : IJogoService
{
    private readonly IRepositorySet _repositorio;

    public JogoService(IRepositorySet repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<(List<JogoDetalheServiceModel> Jogos, int Total)> ListarAsync(ListarJogoFiltroServiceModel filtro)
    {
        // Filtro por gênero: depende de outra tabela, então é resolvido aqui com LINQ.
        // O resultado vai para o SQL como lista de ids, e a paginação continua no banco.
        // null = sem filtro de gênero.
        List<int>? idsJogo = null;
        if (filtro.IdGenero.HasValue)
        {
            var vinculos = await _repositorio.JogoGenero.ListarTodosAsync();
            idsJogo = vinculos
                .Where(vinculo => vinculo.IdGenero == filtro.IdGenero.Value)
                .Select(vinculo => vinculo.IdJogo)
                .Distinct()
                .ToList();
        }

        // Página e tamanho menores que 1 gerariam OFFSET negativo no SQL.
        var pagina = Math.Max(filtro.Pagina, 1);
        var tamanhoPagina = Math.Max(filtro.TamanhoPagina, 1);

        // RN04: a consulta pública mostra só jogos ativos (incluirInativos: false).
        // Administrador ver inativos fica para a aula 6.
        var (jogos, total) = await _repositorio.Jogo.ListarAsync(
            filtro.Busca,
            filtro.IdPlataforma,
            idsJogo,
            incluirInativos: false,
            pagina,
            tamanhoPagina);

        var detalhes = await MontarDetalhesAsync(jogos);
        return (detalhes, total);
    }

    public async Task<JogoDetalheServiceModel> BuscarPorIdAsync(int id)
    {
        var jogo = await BuscarJogoExistenteAsync(id);
        var detalhes = await MontarDetalhesAsync(new List<Jogo> { jogo });
        return detalhes[0];
    }

    public async Task<JogoDetalheServiceModel> InserirAsync(Jogo jogo, IReadOnlyCollection<int> idsGenero)
    {
        var idsGeneroUnicos = idsGenero.Distinct().ToList();
        await ValidarReferenciasAsync(jogo, idsGeneroUnicos);

        jogo.Nome = jogo.Nome.Trim();
        jogo.Ativo = true;
        jogo.DataCadastro = DateTime.UtcNow;

        // Jogo e gêneros numa só transação: ou tudo é gravado, ou nada.
        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout,
        };
        using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                   transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
        {
            jogo.Id = await _repositorio.Jogo.InserirAsync(jogo);
            await _repositorio.JogoGenero.InserirAsync(jogo.Id, idsGeneroUnicos);

            // Só depois da última gravação.
            transactionScope.Complete();
        }

        return await BuscarPorIdAsync(jogo.Id);
    }

    public async Task<JogoDetalheServiceModel> AlterarAsync(Jogo jogo, IReadOnlyCollection<int> idsGenero)
    {
        var jogoAtual = await BuscarJogoExistenteAsync(jogo.Id);

        var idsGeneroUnicos = idsGenero.Distinct().ToList();
        await ValidarReferenciasAsync(jogo, idsGeneroUnicos);

        jogo.Nome = jogo.Nome.Trim();
        // A alteração não muda a situação nem a data de cadastro: quem desativa é o DesativarAsync.
        jogo.Ativo = jogoAtual.Ativo;
        jogo.DataCadastro = jogoAtual.DataCadastro;

        var transactionOptions = new TransactionOptions
        {
            IsolationLevel = IsolationLevel.ReadCommitted,
            Timeout = TransactionManager.DefaultTimeout,
        };
        using (var transactionScope = new TransactionScope(TransactionScopeOption.Required,
                   transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
        {
            await _repositorio.Jogo.AlterarAsync(jogo);
            await _repositorio.JogoGenero.RemoverPorJogoAsync(jogo.Id);
            await _repositorio.JogoGenero.InserirAsync(jogo.Id, idsGeneroUnicos);

            transactionScope.Complete();
        }

        return await BuscarPorIdAsync(jogo.Id);
    }

    public async Task DesativarAsync(int id)
    {
        await BuscarJogoExistenteAsync(id);

        // RN03: exclusão lógica. O jogo nunca é apagado.
        await _repositorio.Jogo.DesativarAsync(id);
    }

    private async Task<Jogo> BuscarJogoExistenteAsync(int id)
    {
        var jogo = await _repositorio.Jogo.BuscarPorIdAsync(id);
        if (jogo is null)
        {
            throw new BusinessRuleException(BusinessErrors.JOGO_NAO_ENCONTRADO);
        }
        return jogo;
    }

    // RN05: plataforma, condição e ao menos um gênero, todos existentes.
    private async Task ValidarReferenciasAsync(Jogo jogo, List<int> idsGenero)
    {
        var plataforma = await _repositorio.Plataforma.BuscarPorIdAsync(jogo.IdPlataforma);
        if (plataforma is null)
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);
        }

        var condicoes = await _repositorio.CondicaoJogo.ListarAsync();
        if (!condicoes.Any(condicao => condicao.Id == jogo.IdCondicaoJogo))
        {
            throw new BusinessRuleException(BusinessErrors.CONDICAO_NAO_ENCONTRADA);
        }

        var idsGeneroExistentes = (await _repositorio.Genero.ListarAsync())
            .Select(genero => genero.Id)
            .ToHashSet();
        if (idsGenero.Count == 0 || idsGenero.Any(idGenero => !idsGeneroExistentes.Contains(idGenero)))
        {
            throw new BusinessRuleException(BusinessErrors.GENERO_NAO_ENCONTRADO);
        }
    }

    // Monta o detalhe de uma lista de jogos sempre com as mesmas 4 consultas,
    // qualquer que seja o tamanho da lista (nunca uma consulta por jogo).
    private async Task<List<JogoDetalheServiceModel>> MontarDetalhesAsync(List<Jogo> jogos)
    {
        var idsJogo = jogos.Select(jogo => jogo.Id).ToList();

        var plataformas = await _repositorio.Plataforma.ListarAsync();
        var condicoes = await _repositorio.CondicaoJogo.ListarAsync();
        var generos = await _repositorio.Genero.ListarAsync();
        var vinculos = await _repositorio.JogoGenero.ListarPorJogosAsync(idsJogo);

        // Dicionários por id: cada busca abaixo é direta, sem percorrer a lista toda.
        var nomePorPlataforma = plataformas.ToDictionary(plataforma => plataforma.Id, plataforma => plataforma.Nome);
        var descricaoPorCondicao = condicoes.ToDictionary(condicao => condicao.Id, condicao => condicao.Descricao);
        var generoPorId = generos.ToDictionary(genero => genero.Id);

        return jogos
            .Select(jogo => new JogoDetalheServiceModel
            {
                Jogo = jogo,
                NomePlataforma = nomePorPlataforma.GetValueOrDefault(jogo.IdPlataforma, string.Empty),
                DescricaoCondicaoJogo = descricaoPorCondicao.GetValueOrDefault(jogo.IdCondicaoJogo, string.Empty),
                Generos = vinculos
                    .Where(vinculo => vinculo.IdJogo == jogo.Id && generoPorId.ContainsKey(vinculo.IdGenero))
                    .Select(vinculo => generoPorId[vinculo.IdGenero])
                    .OrderBy(genero => genero.Nome)
                    .ToList(),
            })
            .ToList();
    }
}
