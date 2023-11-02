using Domain.Exceptions;

namespace Domain.Users;

public sealed record Password
{
    private readonly char[] _specialCharacters = { '!', '?', '@', '#', '$', '%', '^', '&', '*', '(', ')', '.', '/', '[', ']', '-', '_', '+', '=' };
    private const int MinimumPasswordLength = 8;
    private const int MaximumPasswordLength = 200;

    public string Value { get; }

    public Password(string value)
    {
        ThrowIfPasswordIsTooLong(value);
        ThrowIfPasswordIsTooShort(value);
        ThrowIfPasswordDoesntHaveBigLetter(value);
        ThrowIfPasswordDoesntHaveNumber(value);
        ThrowIfPasswordDoesntHaveSpecialCharacter(value);
        Value = value;
    }

    private void ThrowIfPasswordIsTooLong(string value)
    {
        if (value.Length > MaximumPasswordLength)
        {
            throw new PasswordIsTooLongException(MaximumPasswordLength);
        }
    }

    private void ThrowIfPasswordIsTooShort(string value)
    {
        if (value.Length < MinimumPasswordLength)
        {
            throw new PasswordIsTooShortException(
                MinimumPasswordLength,
                MaximumPasswordLength
            );
        }
    }

    private void ThrowIfPasswordDoesntHaveBigLetter(string value)
    {
        if (!value.Any(char.IsUpper))
        {
            throw new PasswordBigLetterMissingException();
        }
    }

    private void ThrowIfPasswordDoesntHaveNumber(string value)
    {
        if (!value.Any(char.IsNumber))
        {
            throw new PasswordNumberCharacterMissingException();
        }
    }

    private void ThrowIfPasswordDoesntHaveSpecialCharacter(string value)
    {
        if (!value.Any(c => _specialCharacters.Contains(c)))
        {
            throw new PasswordSpecialCharacterMissingException();
        }
    }
}