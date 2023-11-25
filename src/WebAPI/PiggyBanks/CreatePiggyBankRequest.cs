namespace WebAPI.PiggyBanks;

public sealed record CreatePiggyBankRequest(
    string Name,
    bool WithGoal,
    decimal Goal
);