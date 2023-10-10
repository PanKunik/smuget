using Domain.Exceptions;

namespace Application.Exceptions;

public sealed class UserWithSameEmailAlreadyExistsException : SmugetException
{
    public UserWithSameEmailAlreadyExistsException()
        : base("User with the same email address already exists.") { }
}