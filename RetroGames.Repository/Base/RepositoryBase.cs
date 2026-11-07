using MySqlConnector;

namespace RetroGames.Repository.Base;

// Métodos que todos os repositórios reaproveitam. Não contém SQL de nenhuma tabela.
// Quem chama usa "await using" na conexão, no comando e no leitor, para que
// sejam fechados mesmo em caso de erro.
public abstract class RepositoryBase
{
    private readonly RepositoryConfig _config;

    protected RepositoryBase(RepositoryConfig config)
    {
        _config = config;
    }

    // Abre uma conexão com o banco. Se houver uma transação aberta pelo Service
    // (TransactionScope), a conexão participa dela automaticamente.
    protected async Task<MySqlConnection> AbrirConexaoAsync()
    {
        var conexao = new MySqlConnection(_config.ConnectionString);
        await conexao.OpenAsync();
        return conexao;
    }

    // Cria um comando com o texto SQL, ligado à conexão.
    protected static MySqlCommand CriarComando(MySqlConnection conexao, string sql)
    {
        return new MySqlCommand(sql, conexao);
    }

    // Adiciona um parâmetro (@nome) ao comando. O valor nunca é concatenado no SQL.
    // null vira DBNull.Value, que é como o ADO.NET representa NULL.
    protected static void AdicionarParametro(MySqlCommand comando, string nome, object? valor)
    {
        comando.Parameters.AddWithValue(nome, valor ?? DBNull.Value);
    }

    // Executa uma gravação (INSERT ou UPDATE) e devolve o número de linhas afetadas.
    // Este é o ÚNICO lugar que converte o erro 1062 (UNIQUE) em RegistroDuplicadoException.
    protected static async Task<int> ExecutarGravacaoAsync(MySqlCommand comando)
    {
        try
        {
            return await comando.ExecuteNonQueryAsync();
        }
        catch (MySqlException ex) when (ex.ErrorCode == MySqlErrorCode.DuplicateKeyEntry)
        {
            throw new RegistroDuplicadoException("Registro duplicado em coluna UNIQUE.", ex);
        }
    }
}
