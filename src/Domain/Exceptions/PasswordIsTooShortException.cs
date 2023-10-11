namespace Domain.Exceptions;

public sealed class PasswordIsTooShortException : SmugetException
{
    public PasswordIsTooShortException()
        : base("Password should be at lease 8 characters long.") { }
}