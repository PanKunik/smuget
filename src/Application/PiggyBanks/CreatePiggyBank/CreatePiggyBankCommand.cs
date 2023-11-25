using Application.Abstractions.CQRS;

namespace Application.PiggyBanks.CreatePiggyBank;

public sealed record CreatePiggyBankCommand(
    string Name,
    bool WithGoal,
    decimal Goal
) : ICommand;