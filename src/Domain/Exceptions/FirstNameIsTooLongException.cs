namespace Domain.Exceptions;

public sealed class FirstNameIsTooLongException : SmugetException
{
    public FirstNameIsTooLongException(int passedLength, int maximumLength)
        : base($"First name can have maximum of {maximumLength} characters. Passed value has {passedLength} characters.")
    {
    }
}
