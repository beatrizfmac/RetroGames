using MySqlConnector;
using RetroGames.Model.Model;
using RetroGames.Repository.Base;

namespace RetroGames.Repository.Repositories;

public class JogoGeneroRepository : RepositoryBase, IJogoGeneroRepository
{
    public JogoGeneroRepository(RepositoryConfig config) : base(config)
    {
    }

    public async Task<List<JogoGenero>> ListarTodosAsync()
    {
        const string sql = @"
            SELECT co_jogo, co_genero
            FROM tb_jogo_genero
            ORDER BY co_jogo, co_genero";

        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, sql);
        return await LerVinculosAsync(comando);
    }

    public async Task<List<JogoGenero>> ListarPorJogosAsync(IReadOnlyCollection<int> idsJogo)
    {
        if (idsJogo.Count == 0)
        {
            return new List<JogoGenero>();
        }

        // Uma única consulta para todos os jogos: um parâmetro por id (@id0, @id1...).
        var nomes = idsJogo.Select((_, indice) => $"@id{indice}");
        var sql = $@"
            SELECT co_jogo, co_genero
            FROM tb_jogo_genero
            WHERE co_jogo IN ({string.Join(", ", nomes)})
            ORDER BY co_jogo, co_genero";

        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, sql);
        var indiceParametro = 0;
        foreach (var id in idsJogo)
        {
            AdicionarParametro(comando, $"@id{indiceParametro}", id);
            indiceParametro++;
        }
        return await LerVinculosAsync(comando);
    }

    public async Task InserirAsync(int idJogo, IReadOnlyCollection<int> idsGenero)
    {
        if (idsGenero.Count == 0)
        {
            return;
        }

        // Um INSERT com várias linhas: (@idJogo, @genero0), (@idJogo, @genero1)...
        var linhas = idsGenero.Select((_, indice) => $"(@idJogo, @genero{indice})");
        var sql = $"INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES {string.Join(", ", linhas)}";

        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@idJogo", idJogo);
        var indiceParametro = 0;
        foreach (var idGenero in idsGenero)
        {
            AdicionarParametro(comando, $"@genero{indiceParametro}", idGenero);
            indiceParametro++;
        }

        await ExecutarGravacaoAsync(comando);
    }

    public async Task RemoverPorJogoAsync(int idJogo)
    {
        const string sql = "DELETE FROM tb_jogo_genero WHERE co_jogo = @idJogo";

        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, sql);
        AdicionarParametro(comando, "@idJogo", idJogo);

        await comando.ExecuteNonQueryAsync();
    }

    private static async Task<List<JogoGenero>> LerVinculosAsync(MySqlCommand comando)
    {
        await using var leitor = await comando.ExecuteReaderAsync();
        var vinculos = new List<JogoGenero>();
        while (await leitor.ReadAsync())
        {
            vinculos.Add(new JogoGenero
            {
                IdJogo = leitor.GetInt32("co_jogo"),
                IdGenero = leitor.GetInt32("co_genero"),
            });
        }
        return vinculos;
    }
}
