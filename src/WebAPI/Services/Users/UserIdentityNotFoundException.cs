using Domain.Exceptions;

namespace WebAPI.Services.Users;

public sealed class UserIdentityNotFoundException
    : SmugetException
{
    public UserIdentityNotFoundException()
        : base("User not authorized.") { }
}