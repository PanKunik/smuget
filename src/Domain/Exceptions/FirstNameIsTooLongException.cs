using Domain.Users;

namespace Domain.Exceptions;

public sealed class FirstNameIsTooLongException
    : InvalidFieldLengthException
{
    public FirstNameIsTooLongException(int maximumLength)
        : base(
            nameof(FirstName),
            maximumLength
        ) { }
}
