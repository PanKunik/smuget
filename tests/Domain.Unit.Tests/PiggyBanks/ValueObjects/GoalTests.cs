using Domain.Exceptions;
using Domain.Piggybanks;

namespace Domain.Unit.Tests.PiggyBanks.ValueObjects;

public sealed class GoalTests
{
    [Fact]
    public void Goal_WhenPassedProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        var createGoal = () => new Goal(2000);

        // Act
        var result = createGoal();

        // Assert
        result
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .Be(2000);
    }

    [Theory]
    [InlineData(-0.123)]
    [InlineData(-1)]
    [InlineData(-123.54)]
    public void Goal_WhenPassedValueLowerThanZero_ShouldThrowInvalidPiggyBankGoalException(decimal value)
    {
        // Arrange
        var createGoal = () => new Goal(value);

        // Act & Assert
        createGoal
            .Should()
            .ThrowExactly<InvalidPiggyBankGoalException>();
    }

    [Fact]
    public void Equals_WhenPassedObjectWithSameValue_ShouldReturnTrue()
    {
        // Arrange
        Goal cut = new(1000m);
        Goal compareTo = new(1000m);

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
        Goal cut = new(1000m);
        Goal compareTo = new(1250m);

        // Act
        var result = cut.Equals(compareTo);

        // Assert
        result
            .Should()
            .BeFalse();
    }
}