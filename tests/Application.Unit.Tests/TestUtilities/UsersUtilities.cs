using Domain.Users;

namespace Application.Unit.Tests.TestUtilities;

public static class UsersUtilities
{
    public static User CreateUser()
        => new(
            new(Constants.Constants.User.Id),
            new(Constants.Constants.User.Email),
            new(Constants.Constants.User.FirstName),
            new(Constants.Constants.User.Password)
        );
}