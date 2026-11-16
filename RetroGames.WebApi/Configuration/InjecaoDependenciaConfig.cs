using RetroGames.Repository;
using RetroGames.Repository.Base;
using RetroGames.Repository.Repositories;
using RetroGames.Services.Services;

namespace RetroGames.WebApi.Configuration;

// Registra na injeção de dependência tudo o que os controllers e Services recebem
// no construtor. O Program.cs chama estes dois métodos.
public static class InjecaoDependenciaConfig
{
    public static IServiceCollection AddRepositorios(this IServiceCollection services, string connectionString)
    {
        // Singleton: uma única instância para toda a aplicação (a connection string não muda).
        services.AddSingleton(new RepositoryConfig { ConnectionString = connectionString });

        // Scoped: uma instância nova por requisição HTTP.
        services.AddScoped<IPlataformaRepository, PlataformaRepository>();
        services.AddScoped<IJogoRepository, JogoRepository>();
        services.AddScoped<IJogoGeneroRepository, JogoGeneroRepository>();
        services.AddScoped<IGeneroRepository, GeneroRepository>();
        services.AddScoped<ICondicaoJogoRepository, CondicaoJogoRepository>();
        services.AddScoped<IRepositorySet, RepositorySet>();

        return services;
    }

    public static IServiceCollection AddServicosDeNegocio(this IServiceCollection services)
    {
        services.AddScoped<IPlataformaService, PlataformaService>();
        services.AddScoped<IJogoService, JogoService>();
        services.AddScoped<IGeneroService, GeneroService>();
        services.AddScoped<ICondicaoJogoService, CondicaoJogoService>();

        return services;
    }
}
