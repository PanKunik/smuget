namespace Domain.Exceptions;

public sealed class EmailIsNullException : SmugetException
{
    public EmailIsNullException()
        : base("Email cannot be null or empty.") { }
}