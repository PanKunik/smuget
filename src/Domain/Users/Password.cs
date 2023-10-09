using Domain.Exceptions;

namespace Domain.Users;

public sealed record Password
{
    private const int MaximumPasswordLength = 200;

    public string Value { get; }

    public Password(
        string value
    )
    {
        ThrowIfPasswordIsTooLong(value);
        Value = value;
    }

    private void ThrowIfPasswordIsTooLong(string value)
    {
        if (value.Length > MaximumPasswordLength)
        {
            throw new PasswordIsTooLongException(
                value.Length,
                MaximumPasswordLength
            );
        }
    }
}