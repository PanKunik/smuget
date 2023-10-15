using Domain.Exceptions;

namespace Application.Exceptions;

public sealed class UserNotFoundException : SmugetException
{
    public UserNotFoundException()
        : base("User with found id doesn't exist.") { }
}