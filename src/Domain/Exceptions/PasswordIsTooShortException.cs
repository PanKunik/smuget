using Domain.Users;

namespace Domain.Exceptions;

public sealed class PasswordIsTooShortException
    : InvalidFieldLengthException
{
    public PasswordIsTooShortException(
        int minLength,
        int maxLength
    )
        : base(
            nameof(Password),
            minLength,
            maxLength
        ) { }
}