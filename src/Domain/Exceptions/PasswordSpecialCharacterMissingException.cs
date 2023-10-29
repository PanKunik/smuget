using Domain.Users;

namespace Domain.Exceptions;

public sealed class PasswordSpecialCharacterMissingException
    : ValidationException
{
    public PasswordSpecialCharacterMissingException()
        : base(
            "Password should contain at least 1 special character.",
            nameof(Password)
        ) { }
}
