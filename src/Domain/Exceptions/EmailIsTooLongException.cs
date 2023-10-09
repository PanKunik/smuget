namespace Domain.Exceptions;

public sealed class EmailIsTooLongException : SmugetException
{
    public EmailIsTooLongException(int passedLength, int maxLength)
        : base($"Email can have maximum of {maxLength} characters. Passed value has {passedLength}.") { }
}
