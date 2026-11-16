using RetroGames.Model.Model;
using RetroGames.Repository;
using RetroGames.Repository.Base;
using RetroGames.Services.Erros;

namespace RetroGames.Services.Services;

// Regras de negócio de plataformas (RN01 e RN02). Não tem SQL nem HTTP:
// consulta e grava sempre pelo repositorio.Plataforma.
public class PlataformaService : IPlataformaService
{
    private readonly IRepositorySet _repositorio;

    public PlataformaService(IRepositorySet repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<List<Plataforma>> ListarAsync()
    {
        return await _repositorio.Plataforma.ListarAsync();
    }

    public async Task<Plataforma> BuscarPorIdAsync(int id)
    {
        var plataforma = await _repositorio.Plataforma.BuscarPorIdAsync(id);
        if (plataforma is null)
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NAO_ENCONTRADA);
        }
        return plataforma;
    }

    public async Task<Plataforma> InserirAsync(Plataforma plataforma)
    {
        plataforma.Nome = plataforma.Nome.Trim();

        // RN01: nome único.
        if (await _repositorio.Plataforma.ExisteComNomeAsync(plataforma.Nome))
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }

        try
        {
            plataforma.Id = await _repositorio.Plataforma.InserirAsync(plataforma);
        }
        catch (RegistroDuplicadoException)
        {
            // Outra requisição gravou o mesmo nome entre a conferência e o INSERT;
            // o UNIQUE do banco recusou. Para quem chamou, é o mesmo erro da RN01.
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }

        return plataforma;
    }

    public async Task<Plataforma> AlterarAsync(Plataforma plataforma)
    {
        // Lança PLATAFORMA_NAO_ENCONTRADA se o id não existir.
        await BuscarPorIdAsync(plataforma.Id);

        plataforma.Nome = plataforma.Nome.Trim();

        // RN01: a própria plataforma é ignorada, então manter o nome não é duplicidade.
        if (await _repositorio.Plataforma.ExisteComNomeAsync(plataforma.Nome, plataforma.Id))
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }

        try
        {
            await _repositorio.Plataforma.AlterarAsync(plataforma);
        }
        catch (RegistroDuplicadoException)
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_NOME_DUPLICADO);
        }

        return plataforma;
    }

    public async Task ExcluirAsync(int id)
    {
        await BuscarPorIdAsync(id);

        // RN02: plataforma com jogos não pode ser excluída.
        if (await _repositorio.Plataforma.PossuiJogosAsync(id))
        {
            throw new BusinessRuleException(BusinessErrors.PLATAFORMA_POSSUI_JOGOS);
        }

        await _repositorio.Plataforma.ExcluirAsync(id);
    }
}
