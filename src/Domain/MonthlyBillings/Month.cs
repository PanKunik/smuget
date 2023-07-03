using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed record Month
{
    public byte Value { get; }

    public Month(byte value)
    {
        if (value is < 1 or > 12)
        {
            throw new InvalidMonthException(value);
        }

        Value = value;
    }
}