using Domain.Users;

namespace Domain.Exceptions;

public sealed class FirstNameIsEmptyException
    : RequiredFieldException
{
    public FirstNameIsEmptyException() 
        : base(nameof(FirstName)) { }
}
