using Domain.Exceptions;
using Domain.Users;

namespace Application.Exceptions;

public sealed class UserNotFoundException
    : NotFoundException
{
    public UserNotFoundException(
        UserId userId
    )
        : base(
            nameof(User),
            nameof(UserId),
            userId.Value.ToString()
        ) { }
}