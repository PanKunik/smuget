using System;
using Domain.Exceptions;
using Domain.Users;

namespace Domain.Unit.Tests.Users.ValueObjects;

public sealed class UserIdTests
{
    [Fact]
    public void UserId_OnSucess_ShouldReturnExpectedObject()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var result = new UserId(
            guid
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .Match<UserId>(
                u => u.Value == guid
            );
    }

    [Fact]
    public void UserId_WhenPassedEmptyGuid_ShouldThrowInvalidUserIdException()
    {
        // Arrange
        var createUserId = () => new UserId(Guid.Empty);

        // Act & Assert
        Assert.Throws<InvalidUserIdException>(createUserId);
    }
}