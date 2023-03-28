using Domain.Exceptions;
using Domain.MonthlyBillings.ValueObjects;

namespace Domain.Unit.Tests.MonthlyBillings.ValueObjects;

public sealed class MonthTests
{
    [Fact]
    public void Month_WhenPassedProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        var createMonth = () => new Month(1);

        // Act
        var result = createMonth();

        // Assert
        result.Should().NotBeNull();
        result.Should().Match<Month>(
            m => m.Value == 1
        );
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public void Month_WhenPassedInvalidMonth_ShouldThrowInvalidMonthException(byte value)
    {
        // Arrange
        var createMonth = () => new Month(value);

        // Act & Assert
        var exception = Assert.Throws<InvalidMonthException>(createMonth);
        exception.Value.Should().Be(value);
    }

    [Fact]
    public void Equals_WhenPassedMonthWithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var compareTo = new Month(5);
        var cut = new Month(5);

        // Act
        var result = cut.Equals(compareTo);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_WhenPassedMonthWithOtherValue_ShouldReturnFalse()
    {
        // Arrange
        var compareTo = new Month(12);
        var cut = new Month(1);

        // Act
        var result = cut.Equals(compareTo);

        // Assert
        result.Should().BeFalse();
    }
}