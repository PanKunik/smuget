using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed record Name
{
    private const byte MaximumNameLength = 50;

    public string Value { get; }

    public Name(
        string value
    )
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new NameIsEmptyException();
        }

        if (value.Length > MaximumNameLength)
        {
            throw new NameIsToLongException(value.Length, MaximumNameLength);
        }

        Value = value;
    }
}