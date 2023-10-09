namespace Domain.Exceptions;

public sealed class EmailIsInvalidException : SmugetException
{
    public EmailIsInvalidException()
        : base("Passed email is not a valid email address.") { }
}