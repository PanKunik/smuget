using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed record Name
{
    private const byte MaximumNameLength = 50;

    public string Value { get; }

    public Name(string value)
    {
        ThrowIfValueIsNullOrWhiteSpace(value);
        ThrowIfValueIsLongerThanMaximumLength(value);
        Value = value;
    }

    private void ThrowIfValueIsNullOrWhiteSpace(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new NameIsEmptyException();
        }
    }

    private void ThrowIfValueIsLongerThanMaximumLength(string value)
    {
        if (value.Length > MaximumNameLength)
        {
            throw new NameIsToLongException(MaximumNameLength);
        }
    }
}