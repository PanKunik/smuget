using Application.Abstractions.CQRS;

namespace Application.PiggyBanks.RemoveTransaction;

public sealed record RemoveTransactionCommand(
    Guid PiggyBankId,
    Guid TransactionId,
    Guid UserId
) : ICommand;