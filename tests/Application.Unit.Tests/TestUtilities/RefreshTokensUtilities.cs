using Domain.RefreshTokens;

namespace Application.Unit.Tests.TestUtilities;

public static class RefreshTokensUtilities
{
    public static RefreshToken CreateRefreshToken()
        => new(
            new(Constants.Constants.RefreshToken.Id),
            Constants.Constants.RefreshToken.Token,
            Constants.Constants.RefreshToken.Expires,
            Constants.Constants.RefreshToken.WasUsed,
            new(Constants.Constants.User.Id)
        );

    public static RefreshToken CreateExpiredRefreshToken()
        => new(
            new(Constants.Constants.RefreshToken.Id),
            Constants.Constants.RefreshToken.Token,
            DateTime.Now.AddDays(-1),
            Constants.Constants.RefreshToken.WasUsed,
            new(Constants.Constants.User.Id)
        );

    public static RefreshToken CreateUsedRefreshToken()
        => new(
            new(Constants.Constants.RefreshToken.Id),
            Constants.Constants.RefreshToken.Token,
            Constants.Constants.RefreshToken.Expires,
            true,
            new(Constants.Constants.User.Id)
        );
}