using Domain.RefreshTokens;

namespace Application.Unit.Tests.TestUtilities;

public static class RefreshTokensUtilities
{
    public static RefreshToken CreateRefreshToken()
        => new(
            new(Constants.Constants.RefreshToken.Id),
            Constants.Constants.RefreshToken.Token,
            Constants.Constants.RefreshToken.CreationDateTime,
            Constants.Constants.RefreshToken.ExpirationDateTime,
            Constants.Constants.RefreshToken.Used,
            Constants.Constants.RefreshToken.Invalidated,
            new(Constants.Constants.User.Id)
        );

    public static RefreshToken CreateExpiredRefreshToken()
        => new(
            new(Constants.Constants.RefreshToken.Id),
            Constants.Constants.RefreshToken.Token,
            Constants.Constants.RefreshToken.CreationDateTime,
            DateTime.Now.AddDays(-1),
            Constants.Constants.RefreshToken.Used,
            Constants.Constants.RefreshToken.Invalidated,
            new(Constants.Constants.User.Id)
        );

    public static RefreshToken CreateUsedRefreshToken()
        => new(
            new(Constants.Constants.RefreshToken.Id),
            Constants.Constants.RefreshToken.Token,
            Constants.Constants.RefreshToken.CreationDateTime,
            Constants.Constants.RefreshToken.ExpirationDateTime,
            true,
            Constants.Constants.RefreshToken.Invalidated,
            new(Constants.Constants.User.Id)
        );

    public static RefreshToken CreateForeignRefreshToken()
        => new(
            new(Constants.Constants.RefreshToken.Id),
            Constants.Constants.RefreshToken.Token,
            Constants.Constants.RefreshToken.CreationDateTime,
            Constants.Constants.RefreshToken.ExpirationDateTime,
            Constants.Constants.RefreshToken.Used,
            Constants.Constants.RefreshToken.Invalidated,
            new(new Guid(21, 12, 5, new byte[] { 0, 9, 1, 3, 2, 5, 32, 22 }))
        );
}