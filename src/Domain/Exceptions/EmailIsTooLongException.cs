using Domain.Users;

namespace Domain.Exceptions;

public sealed class EmailIsTooLongException
    : InvalidFieldLengthException
{
    public EmailIsTooLongException(int maxLength)
        : base(
            nameof(Email),
            maxLength
        ) { }
}
