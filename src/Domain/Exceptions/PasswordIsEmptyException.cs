namespace Domain.Exceptions;

public sealed class PasswordIsEmptyException : SmugetException
{
    public PasswordIsEmptyException()
        : base("Password cannot be null or empty.") { }
}