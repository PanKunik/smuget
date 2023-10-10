using System;
using Domain.Exceptions;
using Domain.Users;

namespace Domain.Unit.Tests.Users;

public sealed class UserTests
{
    [Fact]
    public void User_OnSuccess_ShouldReturnExpectedObject()
    {
        // Act
        var userId = new UserId(Guid.NewGuid());
        var email = new Email("test@example.com");
        var firstName = new FirstName("John");
        var securedPassword = "4a6fafec30def6bfe3ac4e8a538810ff";

        var result = new User(
            userId,
            email,
            firstName,
            securedPassword
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .Match<User>(
                u => u.UserId == userId
                  && u.Email == email
                  && u.FirstName == firstName
                  && u.SecuredPassword == securedPassword
            );
    }

    [Fact]
    public void User_WhenPassedNullUserId_ShouldThrowUserIdIsNullException()
    {
        // Arrange
        var createUser = () => new User(
            null,
            new("example@example.com"),
            new("John"),
            new("P@ssw0rd1.")
        );

        // Act & Assert
        Assert.Throws<UserIdIsNullException>(createUser);
    }

    [Fact]
    public void User_WhenPassedNullEmail_ShouldThrowEmailIsNullException()
    {
        // Arrange
        var createUser = () => new User(
            new(Guid.NewGuid()),
            null,
            new("John"),
            "4a6fafec30def6bfe3ac4e8a538810ff"
        );

        // Act & Assert
        Assert.Throws<EmailIsNullException>(createUser);
    }

    [Fact]
    public void User_WhenPassedNullFirstName_ShouldThrowFirstNameIsNullException()
    {
        // Arrange
        var createUser = () => new User(
            new(Guid.NewGuid()),
            new("john-test@test-example-mono.com"),
            null,
            "4a6fafec30def6bfe3ac4e8a538810ff"
        );

        // Act & Assert
        Assert.Throws<FirstNameIsNullException>(createUser);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void User_WhenPassedNullOrWhiteSpacePassword_ShouldThrowPasswordIsNullException(
        string value
    )
    {
        // Arrange
        var createUser = () => new User(
            new(Guid.NewGuid()),
            new("john-test@test-example-mono.com"),
            new("John"),
            value
        );

        // Act & Assert
        Assert.Throws<PasswordIsEmptyException>(createUser);
    }
}