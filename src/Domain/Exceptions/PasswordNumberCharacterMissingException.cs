namespace Domain.Exceptions;

public sealed class PasswordNumberCharacterMissingException : SmugetException
{
    public PasswordNumberCharacterMissingException()
        : base("Password should contain at least 1 numeric character.") { }
}