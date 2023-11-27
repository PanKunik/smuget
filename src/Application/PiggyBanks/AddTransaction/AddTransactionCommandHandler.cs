using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.PiggyBanks;
using Domain.Repositories;
using Domain.Users;

namespace Application.PiggyBanks.AddTransaction;

public sealed class AddTransactionCommandHandler
    : ICommandHandler<AddTransactionCommand>
{
    private readonly IPiggyBanksRepository _repository;

    public AddTransactionCommandHandler(IPiggyBanksRepository repository)
    {
        _repository = repository;
    }

    public async Task HandleAsync(
        AddTransactionCommand command,
        CancellationToken cancellationToken = default
    )
    {
        PiggyBankId piggyBankId = new(command.PiggyBankId);
        UserId userId = new(command.UserId);

        var entity = await _repository.GetById(
            piggyBankId,
            userId
        );

        if (entity is null)
        {
            throw new PiggyBankNotFoundException(piggyBankId);
        }

        var transaction = new Transaction(
            new(Guid.NewGuid()),
            command.Value,
            command.Date
        );

        entity.AddTransaction(transaction);
        await _repository.Save(entity);
    }
}