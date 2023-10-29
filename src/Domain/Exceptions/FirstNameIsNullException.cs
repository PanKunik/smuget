using Domain.Users;

namespace Domain.Exceptions;

public sealed class FirstNameIsNullException
    : RequiredFieldException
{
    public FirstNameIsNullException()
        : base(nameof(FirstName)) { }
}