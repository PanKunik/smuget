using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.ValueObjects;

public sealed class StateTests
{
    [Fact]
    public void State_Open_ShouldReturnExpectedObject()
    {
        // Arrange
        var createState = () => State.Open;

        // Act
        var result = createState();

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .Match<State>(
                s => s.Id == 1
                  && s.Name == "Open"
            );
    }

    [Fact]
    public void State_Closed_ShouldReturnExpectedObject()
    {
        // Arrange
        var createState = () => State.Closed;

        // Act
        var result = createState();

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .Match<State>(
                s => s.Id == 2
                  && s.Name == "Closed"
            );
    }

    [Fact]
    public void State_WhenComapredToStateWithSameValues_ShouldReturnTrue()
    {
        // Arrange
        var openState = State.Open;
        var compareTo = State.Open;

        // Act
        var result = openState.Equals(compareTo);

        // Assert
        result
            .Should()
            .BeTrue();
    }

    [Fact]
    public void State_WhenComapredToStateWithOtherValues_ShouldReturnFalse()
    {
        // Arrange
        var openState = State.Open;
        var compareTo = State.Closed;

        // Act
        var result = openState.Equals(compareTo);

        // Assert
        result
            .Should()
            .BeFalse();
    }
}