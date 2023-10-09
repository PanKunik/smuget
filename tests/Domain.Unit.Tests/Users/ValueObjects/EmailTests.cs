using Domain.Exceptions;
using Domain.Users;

namespace Domain.Unit.Tests.Users.ValueObjects;

public sealed class EmailTests
{
    [Fact]
    public void Email_OnSuccess_ShouldReturnExpectedObject()
    {
        // Arrange
        var value = "test@example.com";

        // Act
        var result = new Email(
            value
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .Match<Email>(
                e => e.Value == "test@example.com"
            );
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Email_WhenPassedNullOrWhiteSpace_ShouldThrowEmailIsEmptyException(string value)
    {
        // Arrange
        var createEmail = () => new Email(value);

        // Act & Assert
        Assert.Throws<EmailIsEmptyException>(createEmail);
    }

    [Theory]
    [InlineData("abcdefghij.lmnopqrstuvwxyz-abcdefghijklmnopqrstuvwxyz@abc.com")]
    [InlineData("abcd@abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz.com")]
    public void Email_WhenPassedTooLongValue_ShouldThrowEmailIsTooLongException(string value)
    {
        // Arrange
        var createEmail = () => new Email(
            value
        );

        // Act & Assert
        Assert.Throws<EmailIsTooLongException>(createEmail);
    }

    [Theory]
    [InlineData("abc@pl")]
    [InlineData("abc123.pl")]
    [InlineData("@abcasd.12")]
    public void Email_WhenPassedInvalidEmail_ShouldThrowEmailIsInvalidExcpetion(string value)
    {
        // Arrange
        var createEmail = () => new Email(
            value
        );

        // Act & Assert
        Assert.Throws<EmailIsInvalidException>(createEmail);
    }
}