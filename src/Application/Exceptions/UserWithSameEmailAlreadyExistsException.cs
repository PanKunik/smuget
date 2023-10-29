using Domain.Exceptions;
using Domain.Users;

namespace Application.Exceptions;

public sealed class UserWithSameEmailAlreadyExistsException
    : ConflictException
{
    public UserWithSameEmailAlreadyExistsException(
        Email email
    )
        : base(
            nameof(User),
            nameof(Email),
            email.Value
        ) { }
}