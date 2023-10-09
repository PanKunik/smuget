namespace Domain.Exceptions;

public sealed class PasswordIsNullException : SmugetException
{
    public PasswordIsNullException()
        : base("Password cannot be null or empty.") { }
}