using Application.Identity.Logout;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.Identity.Logout;

public static class LogoutCommandUtilities
{
    public static LogoutCommand CreateCommand()
        => new(
            Constants.RefreshToken.Token,
            Constants.User.Id
        );
}