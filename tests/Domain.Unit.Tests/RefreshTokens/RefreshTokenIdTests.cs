using System;
using Domain.Exceptions;
using Domain.RefreshTokens;

namespace Domain.Unit.Tests.RefreshTokens;

public sealed class RefreshTokenIdTests
{
    [Fact]
    public void RefreshTokenId_WhenCalled_ShouldReturnExpectedObject()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var result = new RefreshTokenId(
            guid
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .Match<RefreshTokenId>(
                r => r.Value == guid
            );
    }

    [Fact]
    public void RefreshToken_WhenPassedValueEqualToEmptyGuid_ShouldThrowInvalidRefreshTokenIdException()
    {
        // Arrange
        var cut = () => new RefreshTokenId(
            Guid.Empty
        );

        // Act & Assert
        Assert.Throws<InvalidRefreshTokenIdException>(cut);
    }
}