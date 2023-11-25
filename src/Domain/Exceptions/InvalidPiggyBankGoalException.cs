using Domain.PiggyBanks;

namespace Domain.Exceptions;

public sealed class InvalidPiggyBankGoalException
    : ValidationException
{
    public InvalidPiggyBankGoalException()
        : base(
            $"PiggyBank's goal must be a non-negative value.",
            nameof(Goal)
        ) { }
}
