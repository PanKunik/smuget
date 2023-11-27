using Application.Abstractions.CQRS;

namespace Application.PiggyBanks.AddTransaction;

public sealed record AddTransactionCommand(
    Guid PiggyBankId,
    decimal Value,
    DateOnly Date,
    Guid UserId
) : ICommand;