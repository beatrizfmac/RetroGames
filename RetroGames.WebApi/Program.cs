using RetroGames.WebApi.Configuration;
using RetroGames.WebApi.Middlewares;

// Carrega o .env da raiz do repositório (TraversePath sobe as pastas até encontrá-lo).
// Precisa vir antes do CreateBuilder: cada linha do .env vira variável de ambiente,
// e ConnectionStrings__Default passa a ser lida como ConnectionStrings:Default.
DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string não encontrada. Copie o .env.example para .env, na raiz do repositório, " +
        "e preencha a linha ConnectionStrings__Default.");
}

// Injeção de dependência
builder.Services.AddRepositorios(connectionString);
builder.Services.AddServicosDeNegocio();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Pipeline: a sequência de etapas por onde toda requisição passa.
// O tratamento de exceções vem primeiro, para pegar erros de todas as etapas seguintes.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    // Descrição da API em /openapi/v1.json e página de testes em /swagger.
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "RetroGames API"));
}

app.MapControllers();

app.Run();
