using Domain.Users;

namespace Domain.Exceptions;

public sealed class PasswordNumberCharacterMissingException
    : ValidationException
{
    public PasswordNumberCharacterMissingException()
        : base(
            "Password should contain at least 1 numeric character.",
            nameof(Password)
        ) { }
}