using Domain.Exceptions;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.ValueObjects;

public sealed class NameTests
{
    [Theory]
    [InlineData("a")]
    [InlineData("AY5txhr2Qo")]
    [InlineData("Orbe2an9BBC5NdkkLQGOdKzMdPO8IBJ4lddxDNlFUVMzBzHzi2")]
    public void Name_WhenPassedProperData_ShouldReturnExpectedObject(string value)
    {
        // Arrange
        var createName = () => new Name(value);

        // Act
        var result = createName();

        // Assert
        result
            .Should()
            .NotBeNull();
        
        result
            .Should()
            .Match<Name>(
                d => d.Value == value
            );
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Name_WhenPassedNullOrEmptyString_ShouldThrowNameIsEmptyException(string value)
    {
        // Arrange
        var createName = () => new Name(value);

        // Act & Assert
        Assert.Throws<NameIsEmptyException>(createName);
    }

    [Theory]
    [InlineData("ovZ4pgjcinz7VvvK5HfvQzjsyq8O7hSx06bU51it47jsqHbbbLD")]
    [InlineData("ByBxln8bpcyazK7HL3Qq47iNAdTdHqQARlYSwQoqMsMqeqOI4JrJ")]
    public void Name_WhenPassedTooLongValue_ShouldThrowNameIsTOoLongException(string value)
    {
        // Arrange
        var createName = () => new Name(value);

        // Act & Assert
        Assert.Throws<NameIsToLongException>(createName);
    }
}