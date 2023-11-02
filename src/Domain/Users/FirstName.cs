using Domain.Exceptions;

namespace Domain.Users;

public sealed record FirstName
{
    private const int MaximumLengthOfFirstName = 50;

    public string Value { get; }

    public FirstName(string value)
    {
        ThrowIfFirstNameIsNullOrWhiteSpace(value);
        ThrowIfFirstNameIsTooLong(value);
        Value = value;
    }

    private void ThrowIfFirstNameIsNullOrWhiteSpace(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new FirstNameIsEmptyException();
        }
    }

    private void ThrowIfFirstNameIsTooLong(string value)
    {
        if (value.Length > MaximumLengthOfFirstName)
        {
            throw new FirstNameIsTooLongException(MaximumLengthOfFirstName);
        }
    }
}