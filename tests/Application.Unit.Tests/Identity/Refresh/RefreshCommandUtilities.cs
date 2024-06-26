using Application.Unit.Tests.TestUtilities.Constants;
using Application.Identity.Refresh;

namespace Application.Unit.Tests.Identity.Refresh;

public static class RefreshCommandUtilities
{
    public static RefreshCommand CreateCommand()
        => new(
            Constants.User.AccessToken,
            Constants.RefreshToken.Token,
            Constants.User.IpAddress
        );
}