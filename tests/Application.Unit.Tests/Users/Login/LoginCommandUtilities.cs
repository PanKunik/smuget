using Application.Unit.Tests.TestUtilities.Constants;
using Application.Users.Login;

namespace Application.Unit.Tests.Users.Login;

public static class LoginCommandUtilities
{
    public static LoginCommand CreateCommand()
        => new(
            Constants.User.Email,
            Constants.User.Password
        );
}