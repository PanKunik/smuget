using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed record Year
{
    public ushort Value { get; }

    public Year(ushort value)
    {
        if (value <= 1900)
        {
            throw new InvalidYearException(value);
        }

        Value = value;
    }
}