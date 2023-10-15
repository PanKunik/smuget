using Application.Unit.Tests.TestUtilities.Constants;
using Application.Users.Refresh;

namespace Application.Unit.Tests.Users.Refresh;

public static class RefreshCommandUtilities
{
    public static RefreshCommand CreateCommand()
        => new(
            Constants.RefreshToken.Token
        );
}