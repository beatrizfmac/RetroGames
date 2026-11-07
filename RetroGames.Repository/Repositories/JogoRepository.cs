using MySqlConnector;
using RetroGames.Model.Model;
using RetroGames.Repository.Base;

namespace RetroGames.Repository.Repositories;

public class JogoRepository : RepositoryBase, IJogoRepository
{
    private const string Colunas = @"
        co_seq_jogo, no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque,
        st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo";

    public JogoRepository(RepositoryConfig config) : base(config)
    {
    }

    public async Task<(List<Jogo> Jogos, int Total)> ListarAsync(
        string? busca,
        int? idPlataforma,
        IReadOnlyCollection<int>? idsJogo,
        bool incluirInativos,
        int pagina,
        int tamanhoPagina)
    {
        // Lista de ids vazia: nenhum jogo atende ao filtro.
        if (idsJogo is not null && idsJogo.Count == 0)
        {
            return (new List<Jogo>(), 0);
        }

        // Cada filtro só vale quando foi informado ("@x IS NULL OR ...").
        // A contagem e a página usam exatamente o mesmo WHERE.
        var where = @"
            WHERE (@incluirInativos = TRUE OR st_ativo = TRUE)
              AND (@busca IS NULL OR no_jogo LIKE CONCAT('%', @busca, '%'))
              AND (@idPlataforma IS NULL OR co_plataforma = @idPlataforma)";

        // Lista de ids: um parâmetro por id (@id0, @id1...). Só os NOMES dos
        // parâmetros entram no texto do SQL; os valores continuam como parâmetros.
        if (idsJogo is not null)
        {
            var nomes = idsJogo.Select((_, indice) => $"@id{indice}");
            where += $" AND co_seq_jogo IN ({string.Join(", ", nomes)})";
        }

        var sqlTotal = $"SELECT COUNT(*) FROM tb_jogo {where}";
        var sqlPagina = $@"
            SELECT {Colunas}
            FROM tb_jogo
            {where}
            ORDER BY no_jogo, co_seq_jogo
            LIMIT @tamanhoPagina OFFSET @offset";

        await using var conexao = await AbrirConexaoAsync();

        int total;
        await using (var comandoTotal = CriarComando(conexao, sqlTotal))
        {
            AdicionarFiltros(comandoTotal, busca, idPlataforma, idsJogo, incluirInativos);
            total = Convert.ToInt32(await comandoTotal.ExecuteScalarAsync());
        }

        var jogos = new List<Jogo>();
        await using (var comandoPagina = CriarComando(conexao, sqlPagina))
        {
            AdicionarFiltros(comandoPagina, busca, idPlataforma, idsJogo, incluirInativos);
            AdicionarParametro(comandoPagina, "@tamanhoPagina", tamanhoPagina);
            AdicionarParametro(comandoPagina, "@offset", (pagina - 1) * tamanhoPagina);

            await using var leitor = await comandoPagina.ExecuteReaderAsync();
            while (await leitor.ReadAsync())
            {
                jogos.Add(Mapear(leitor));
            }
        }

        return (jogos, total);
    }

    public async Task<Jogo?> BuscarPorIdAsync(int id)
    {
        var sql = $"SELECT {Colunas} FROM tb_jogo WHERE co_seq_jogo = @id";

        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@id", id);
        await using var leitor = await comando.ExecuteReaderAsync();

        return await leitor.ReadAsync() ? Mapear(leitor) : null;
    }

    public async Task<int> InserirAsync(Jogo jogo)
    {
        const string sql = @"
            INSERT INTO tb_jogo (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque,
                                 st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo)
            VALUES (@nome, @descricao, @anoLancamento, @preco, @estoque,
                    @ativo, @dataCadastro, @idPlataforma, @idCondicaoJogo)";

        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@nome", jogo.Nome);
        AdicionarParametro(comando, "@descricao", jogo.Descricao);
        AdicionarParametro(comando, "@anoLancamento", jogo.AnoLancamento);
        AdicionarParametro(comando, "@preco", jogo.Preco);
        AdicionarParametro(comando, "@estoque", jogo.Estoque);
        AdicionarParametro(comando, "@ativo", jogo.Ativo);
        AdicionarParametro(comando, "@dataCadastro", jogo.DataCadastro);
        AdicionarParametro(comando, "@idPlataforma", jogo.IdPlataforma);
        AdicionarParametro(comando, "@idCondicaoJogo", jogo.IdCondicaoJogo);

        await ExecutarGravacaoAsync(comando);
        return (int)comando.LastInsertedId;
    }

    public async Task AlterarAsync(Jogo jogo)
    {
        // dt_cadastro não muda na alteração.
        const string sql = @"
            UPDATE tb_jogo
            SET no_jogo = @nome,
                ds_jogo = @descricao,
                nu_ano_lancamento = @anoLancamento,
                vl_preco = @preco,
                qt_estoque = @estoque,
                st_ativo = @ativo,
                co_plataforma = @idPlataforma,
                co_condicao_jogo = @idCondicaoJogo
            WHERE co_seq_jogo = @id";

        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@id", jogo.Id);
        AdicionarParametro(comando, "@nome", jogo.Nome);
        AdicionarParametro(comando, "@descricao", jogo.Descricao);
        AdicionarParametro(comando, "@anoLancamento", jogo.AnoLancamento);
        AdicionarParametro(comando, "@preco", jogo.Preco);
        AdicionarParametro(comando, "@estoque", jogo.Estoque);
        AdicionarParametro(comando, "@ativo", jogo.Ativo);
        AdicionarParametro(comando, "@idPlataforma", jogo.IdPlataforma);
        AdicionarParametro(comando, "@idCondicaoJogo", jogo.IdCondicaoJogo);

        await ExecutarGravacaoAsync(comando);
    }

    public async Task DesativarAsync(int id)
    {
        // Exclusão lógica (RN03): o jogo nunca é apagado.
        const string sql = "UPDATE tb_jogo SET st_ativo = FALSE WHERE co_seq_jogo = @id";

        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@id", id);

        await ExecutarGravacaoAsync(comando);
    }

    private static void AdicionarFiltros(
        MySqlCommand comando,
        string? busca,
        int? idPlataforma,
        IReadOnlyCollection<int>? idsJogo,
        bool incluirInativos)
    {
        AdicionarParametro(comando, "@incluirInativos", incluirInativos);
        AdicionarParametro(comando, "@busca", string.IsNullOrWhiteSpace(busca) ? null : busca.Trim());
        AdicionarParametro(comando, "@idPlataforma", idPlataforma);

        if (idsJogo is not null)
        {
            var indice = 0;
            foreach (var id in idsJogo)
            {
                AdicionarParametro(comando, $"@id{indice}", id);
                indice++;
            }
        }
    }

    private static Jogo Mapear(MySqlDataReader leitor)
    {
        var indiceDescricao = leitor.GetOrdinal("ds_jogo");
        return new Jogo
        {
            Id = leitor.GetInt32("co_seq_jogo"),
            Nome = leitor.GetString("no_jogo"),
            Descricao = leitor.IsDBNull(indiceDescricao) ? null : leitor.GetString(indiceDescricao),
            AnoLancamento = leitor.GetInt32("nu_ano_lancamento"),
            Preco = leitor.GetDecimal("vl_preco"),
            Estoque = leitor.GetInt32("qt_estoque"),
            Ativo = leitor.GetBoolean("st_ativo"),
            DataCadastro = leitor.GetDateTime("dt_cadastro"),
            IdPlataforma = leitor.GetInt32("co_plataforma"),
            IdCondicaoJogo = leitor.GetInt32("co_condicao_jogo"),
        };
    }
}
