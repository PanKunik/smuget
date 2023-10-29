using Domain.Users;

namespace Domain.Exceptions;

public sealed class PasswordIsTooLongException
    : InvalidFieldLengthException
{
    public PasswordIsTooLongException(int maxLength)
        : base(
            nameof(Password),
            maxLength
        ) { }
}