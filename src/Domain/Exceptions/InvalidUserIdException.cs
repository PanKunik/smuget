using Domain.Users;

namespace Domain.Exceptions;

public sealed class InvalidUserIdException
    : RequiredFieldException
{
    public InvalidUserIdException()
        : base(nameof(UserId)) { }
}
