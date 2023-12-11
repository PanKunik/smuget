using Application.Abstractions.CQRS;

namespace Application.PiggyBanks.UpdateTransaction;

public sealed record UpdateTransactionCommand(
    Guid PiggyBankId,
    Guid TransactionId,
    Guid UserId,
    decimal Value,
    DateOnly Date
) : ICommand;
