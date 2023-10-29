using Domain.Users;

namespace Domain.Exceptions;

public sealed class PasswordIsNullException
    : RequiredFieldException
{
    public PasswordIsNullException()
        : base(nameof(Password)) { }
}