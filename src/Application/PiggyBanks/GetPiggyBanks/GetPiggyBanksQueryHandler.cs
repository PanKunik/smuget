using Application.Abstractions.CQRS;
using Application.PiggyBanks.Mappings;
using Domain.Repositories;
using Domain.Users;

namespace Application.PiggyBanks.GetPiggyBanks;

public sealed class GetPiggyBanksQueryHandler
    : IQueryHandler<GetPiggyBanksQuery, IEnumerable<PiggyBankDTO?>>
{
    private readonly IPiggyBanksRepository _repository;

    public GetPiggyBanksQueryHandler(IPiggyBanksRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PiggyBankDTO?>> HandleAsync(
        GetPiggyBanksQuery query,
        CancellationToken token = default
    )
    {
        UserId userId = new(query.UserId);

        var entities = await _repository.GetAll(
            userId,
            token
        );

        return entities?
            .Select(p => p?.ToDTO())
            .ToList() ?? new List<PiggyBankDTO?>();
    }
}