using Domain.Users;

namespace Domain.Exceptions;

public sealed class PasswordIsEmptyException
    : RequiredFieldException
{
    public PasswordIsEmptyException()
        : base(nameof(Password)) { }
}