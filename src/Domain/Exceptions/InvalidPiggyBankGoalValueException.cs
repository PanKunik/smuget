using Domain.PiggyBanks;

namespace Domain.Exceptions;

public sealed class InvalidPiggyBankGoalValueException
    : ValidationException
{
    public InvalidPiggyBankGoalValueException()
        : base(
            $"When Piggy bank is set `withGoal` the `goal` should be greater than 0, otherwise it should be 0.",
            nameof(Goal)
        ) { }
}
