namespace Domain.Exceptions;

public sealed class PasswordBigLetterMissingException : SmugetException
{
    public PasswordBigLetterMissingException()
        : base("Password should contain at least 1 big letter.") { }
}
