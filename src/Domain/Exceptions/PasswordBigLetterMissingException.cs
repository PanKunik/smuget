using Domain.Users;

namespace Domain.Exceptions;

public sealed class PasswordBigLetterMissingException
    : ValidationException
{
    public PasswordBigLetterMissingException()
        : base(
            "Password should contain at least 1 big letter.",
            nameof(Password)
        ) { }
}
