namespace RetroGames.Repository.Base;

// Guarda a connection string. O valor vem do .env (ConnectionStrings__Default),
// lido pela API e entregue a esta classe na aula 4.
public class RepositoryConfig
{
    public string ConnectionString { get; set; } = string.Empty;
}
