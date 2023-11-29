using Application.Abstractions.CQRS;
using Application.Exceptions;
using Application.PiggyBanks.Mappings;
using Domain.PiggyBanks;
using Domain.Repositories;
using Domain.Users;

namespace Application.PiggyBanks.GetPiggyBankById;

public sealed class GetPiggyBankByIdQueryHandler
    : IQueryHandler<GetPiggyBankByIdQuery, PiggyBankDTO>
{
    private readonly IPiggyBanksRepository _repository;

    public GetPiggyBankByIdQueryHandler(IPiggyBanksRepository repository)
    {
        _repository = repository;
    }

    public async Task<PiggyBankDTO> HandleAsync(
        GetPiggyBankByIdQuery query,
        CancellationToken token = default
    )
    {
        PiggyBankId piggyBankId = new(query.PiggyBankId);
        UserId userId = new(query.UserId);

        var entity = await _repository.GetById(
            piggyBankId,
            userId,
            token
        );
        
        if (entity is null)
        {
            throw new PiggyBankNotFoundException(piggyBankId);
        }

        return entity?.ToDTO();
    }
}