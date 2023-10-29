using Domain.Users;

namespace Domain.Exceptions;

public sealed class UserIdIsNullException
    : RequiredFieldException
{
    public UserIdIsNullException()
        : base(nameof(UserId)) { }
}