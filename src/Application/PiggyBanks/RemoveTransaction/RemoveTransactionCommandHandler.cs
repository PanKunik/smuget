using Application.Abstractions.CQRS;
using Application.Exceptions;
using Domain.PiggyBanks;
using Domain.Repositories;
using Domain.Users;

namespace Application.PiggyBanks.RemoveTransaction;

public sealed class RemoveTransactionCommandHandler
    : ICommandHandler<RemoveTransactionCommand>
{
    private readonly IPiggyBanksRepository _repository;

    public RemoveTransactionCommandHandler(IPiggyBanksRepository repository)
    {
        _repository = repository
            ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task HandleAsync(
        RemoveTransactionCommand command,
        CancellationToken cancellationToken = default
    )
    {
        PiggyBankId piggyBankId = new(command.PiggyBankId);
        TransactionId transactionId = new(command.TransactionId);
        UserId userId = new(command.UserId);

        var entity = await _repository.GetById(
            piggyBankId,
            userId
        );

        if (entity is null)
        {
            throw new PiggyBankNotFoundException(piggyBankId);
        }

        entity.RemoveTransaction(transactionId);
        await _repository.Save(entity);
    }
}
