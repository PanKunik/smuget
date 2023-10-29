using Domain.Exceptions;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.ValueObjects;

public sealed class YearTests
{
    [Fact]
    public void Year_WhenPassedProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        var createYear = () => new Year(2005);

        // Act
        var result = createYear();

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .Match<Year>(
                y => y.Value == 2005
            );
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1899)]
    [InlineData(1900)]
    public void Year_WhenPassedYearLowerThan1900_ShouldThrowInvalidYearExcetpion(ushort value)
    {
        // Arrange
        var createYear = () => new Year(value);

        // Act & Assert
        var exception = Assert.Throws<InvalidYearException>(createYear);
        exception.Year
            .Should()
            .Be(value);
    }

    [Fact]
    public void Equals_WhenPassedObjectWithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var compareTo = new Year(2000);
        var cut = new Year(2000);

        // Act
        var result = cut.Equals(compareTo);

        // Assert
        result
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Equals_WhenPassedObjectWithOtherValue_ShouldReturnFalse()
    {
        // Arrange
        var compareTo = new Year(2001);
        var cut = new Year(2000);

        // Act
        var result = cut.Equals(compareTo);

        // Assert
        result
            .Should()
            .BeFalse();
    }
}