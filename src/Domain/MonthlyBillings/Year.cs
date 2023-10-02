using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed record Year
{
    public int Value { get; }

    public Year(int value)
    {
        ThrowIfYearTooLow(value);

        Value = value;
    }

    private void ThrowIfYearTooLow(int value)
    {
        if (value <= 1900)
        {
            throw new InvalidYearException(value);
        }
    }
}