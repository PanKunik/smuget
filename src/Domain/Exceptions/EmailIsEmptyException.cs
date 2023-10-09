namespace Domain.Exceptions;

public sealed class EmailIsEmptyException : SmugetException
{
    public EmailIsEmptyException()
        : base("Email cannot be null or empty.") { }
}