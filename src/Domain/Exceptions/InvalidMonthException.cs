using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class InvalidMonthException
    : ValidationException
{
    public int Value { get; }

    public InvalidMonthException(int value)
        : base(
            $"Month must be an integer number from 1 to 12. Passed: {value}.",
            nameof(Month)
        )
    {
        Value = value;
    }
}