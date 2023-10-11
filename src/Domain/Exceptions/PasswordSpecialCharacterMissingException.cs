namespace Domain.Exceptions;

public sealed class PasswordSpecialCharacterMissingException : SmugetException
{
    public PasswordSpecialCharacterMissingException()
        : base("Password should contain at least 1 special character.") { }
}
