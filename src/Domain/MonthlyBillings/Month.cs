using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed record Month
{
    public int Value { get; }

    public Month(int value)
    {
        if (value is < 1 or > 12)
        {
            throw new InvalidMonthException(value);
        }

        Value = value;
    }
}