using Domain.Exceptions;

namespace Domain.PiggyBanks;

public sealed record Goal
{
    public decimal Value { get; }

    public Goal(decimal value)
    {
        ThrowIfValueIsLowerThanZero(value);
        Value = value;
    }

    private static void ThrowIfValueIsLowerThanZero(decimal value)
    {
        if (value < 0m)
        {
            throw new InvalidPiggyBankGoalException();
        }
    }
}