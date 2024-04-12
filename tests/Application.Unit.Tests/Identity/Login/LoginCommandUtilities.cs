using Application.Unit.Tests.TestUtilities.Constants;
using Application.Identity.Login;

namespace Application.Unit.Tests.Identity.Login;

public static class LoginCommandUtilities
{
    public static LoginCommand CreateCommand()
        => new(
            Constants.User.Email,
            Constants.User.Password,
            Constants.User.IpAddress
        );
}