namespace Domain.Exceptions;

public sealed class PasswordIsTooLongException : SmugetException
{
    public PasswordIsTooLongException(int passedLength, int maximumLength)
        : base($"Password can have maximum of {maximumLength} characters. Passed value has {passedLength} characters.")
    {
    }
}