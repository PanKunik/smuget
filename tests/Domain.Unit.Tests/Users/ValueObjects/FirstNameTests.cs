using Domain.Exceptions;
using Domain.Users;

namespace Domain.Unit.Tests.Users.ValueObjects;

public sealed class FirstNameTests
{
    [Fact]
    public void FirstName_OnSuccess_ShouldReturnExpectedObject()
    {
        // Arrange
        var value = "Joe";

        // Act
        var result = new FirstName(
            value
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .Match<FirstName>(
                f => f.Value == value
            );
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void FirstName_WhenPassedNullOrWhiteSpace_ShouldThrowFirstNameIsEmptyException(string value)
    {
        // Arrange
        var createFirstName = () => new FirstName(
            value
        );

        // Act & Assert
        Assert.Throws<FirstNameIsEmptyException>(createFirstName);
    }

    [Theory]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxy")]
    [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmno")]
    public void FirstName_WhenPassedTooLongValue_ShouldThrowFirstNameIsTooLongException(string value)
    {
        // Arrange
        var createFirstName = () => new FirstName(
            value
        );

        // Act & Assert
        Assert.Throws<FirstNameIsTooLongException>(createFirstName);
    }
}