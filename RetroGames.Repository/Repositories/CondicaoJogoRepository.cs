using RetroGames.Model.Model;
using RetroGames.Repository.Base;

namespace RetroGames.Repository.Repositories;

public class CondicaoJogoRepository : RepositoryBase, ICondicaoJogoRepository
{
    public CondicaoJogoRepository(RepositoryConfig config) : base(config)
    {
    }

    public async Task<List<CondicaoJogo>> ListarAsync()
    {
        const string sql = @"
            SELECT co_condicao_jogo, ds_condicao_jogo
            FROM tb_condicao_jogo
            ORDER BY co_condicao_jogo";

        await using var conexao = await AbrirConexaoAsync();
        await using var comando = CriarComando(conexao, sql);
        await using var leitor = await comando.ExecuteReaderAsync();

        var condicoes = new List<CondicaoJogo>();
        while (await leitor.ReadAsync())
        {
            condicoes.Add(new CondicaoJogo
            {
                Id = leitor.GetInt32("co_condicao_jogo"),
                Descricao = leitor.GetString("ds_condicao_jogo"),
            });
        }
        return condicoes;
    }
}
