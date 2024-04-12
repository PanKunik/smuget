using Domain.Exceptions;
using Domain.RefreshTokens;
using Domain.Unit.Tests.RefreshTokens.TestUtilities;

namespace Domain.Unit.Tests.RefreshTokens;

public sealed class RefreshTokenTests
{
    [Fact]
    public void RefreshToken_WhenCalled_ShouldReturnExpectedObject()
    {
        // Act
        var cut = new RefreshToken(
            Constants.RefreshToken.Id,
            Constants.RefreshToken.Token,
            Constants.RefreshToken.JwtId,
            Constants.RefreshToken.CreationDateTime,
            Constants.RefreshToken.ExpirationDateTime,
            Constants.RefreshToken.IssuedFrom,
            Constants.RefreshToken.Used,
            Constants.RefreshToken.Invalidated,
            Constants.RefreshToken.UserId
        );

        // Assert
        cut
            .Should()
            .NotBeNull();

        cut
            .Should()
            .Match<RefreshToken>(
                r => r.Id == Constants.RefreshToken.Id
            );
    }

    [Fact]
    public void RefreshToken_WhenPassedNullRefreshTokenId_ShouldThrowRefreshTokenIdIsNullException()
    {
        // Arrange
        var cut = () => new RefreshToken(
            null,
            Constants.RefreshToken.Token,
            Constants.RefreshToken.JwtId,
            Constants.RefreshToken.CreationDateTime,
            Constants.RefreshToken.ExpirationDateTime,
            Constants.RefreshToken.IssuedFrom,
            Constants.RefreshToken.Used,
            Constants.RefreshToken.Invalidated,
            Constants.RefreshToken.UserId
        );

        // Act & Assert
        Assert.Throws<RefreshTokenIdIsNullException>(cut);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void RefreshToken_WhenPassedNullOrWhiteSpaceToken_ShouldThrowInvalidTokenException(string value)
    {
        // Arrange
        var cut = () => new RefreshToken(
            Constants.RefreshToken.Id,
            value,
            Constants.RefreshToken.JwtId,
            Constants.RefreshToken.CreationDateTime,
            Constants.RefreshToken.ExpirationDateTime,
            Constants.RefreshToken.IssuedFrom,
            Constants.RefreshToken.Used,
            Constants.RefreshToken.Invalidated,
            Constants.RefreshToken.UserId
        );

        // Act & Assert
        Assert.Throws<InvalidTokenException>(cut);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void RefreshToken_WhenPassedNullOrWhiteSpaceIssuedFrom_ShouldThrowUserIdIsNullException(string issuedFrom)
    {
        // Arrange
        var cut = () => new RefreshToken(
            Constants.RefreshToken.Id,
            Constants.RefreshToken.Token,
            Constants.RefreshToken.JwtId,
            Constants.RefreshToken.CreationDateTime,
            Constants.RefreshToken.ExpirationDateTime,
            issuedFrom,
            Constants.RefreshToken.Used,
            Constants.RefreshToken.Invalidated,
            Constants.RefreshToken.UserId
        );

        // Act & Assert
        Assert.Throws<InvalidIpAddressException>(cut);
    }

    [Fact]
    public void RefreshToken_WhenPassedNullUserId_ShouldThrowUserIdIsNullException()
    {
        // Arrange
        var cut = () => new RefreshToken(
            Constants.RefreshToken.Id,
            Constants.RefreshToken.Token,
            Constants.RefreshToken.JwtId,
            Constants.RefreshToken.CreationDateTime,
            Constants.RefreshToken.ExpirationDateTime,
            Constants.RefreshToken.IssuedFrom,
            Constants.RefreshToken.Used,
            Constants.RefreshToken.Invalidated,
            null
        );

        // Act & Assert
        Assert.Throws<UserIdIsNullException>(cut);
    }

    [Fact]
    public void RefreshToken_IsActive_ShouldReturnTrue()
    {
        // Arrange
        var cut = new RefreshToken(
            Constants.RefreshToken.Id,
            Constants.RefreshToken.Token,
            Constants.RefreshToken.JwtId,
            Constants.RefreshToken.CreationDateTime,
            Constants.RefreshToken.ExpirationDateTime,
            Constants.RefreshToken.IssuedFrom,
            Constants.RefreshToken.Used,
            Constants.RefreshToken.Invalidated,
            Constants.RefreshToken.UserId
        );

        // Act
        bool result = cut.IsActive();

        // Assert
        result
            .Should()
            .BeTrue();
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(true, true)]
    [InlineData(false, true)]
    public void RefreshToken_IsActive_ShouldReturnFalse(bool used, bool invalidated)
    {
        // Arrange
        var cut = new RefreshToken(
            Constants.RefreshToken.Id,
            Constants.RefreshToken.Token,
            Constants.RefreshToken.JwtId,
            Constants.RefreshToken.CreationDateTime,
            Constants.RefreshToken.ExpirationDateTime,
            Constants.RefreshToken.IssuedFrom,
            used,
            invalidated,
            Constants.RefreshToken.UserId
        );

        // Act
        bool result = cut.IsActive();

        // Assert
        result
            .Should()
            .BeFalse();
    }
}