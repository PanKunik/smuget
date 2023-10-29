using Domain.Users;

namespace Domain.Exceptions;

public sealed class EmailIsInvalidException
    : ValidationException
{
    public EmailIsInvalidException()
        : base(
            "Passed email is not a valid email address.",
            nameof(Email)
        ) { }
}