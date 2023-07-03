using System;
using Domain.Exceptions;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.ValueObjects;

public sealed class ExpenseIdTests
{
    [Fact]
    public void ExpenseId_WhenPassedProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        var guid = Guid.NewGuid();

        var createExpenseId = () => new ExpenseId(guid);

        // Act
        var result = createExpenseId();

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .Match<ExpenseId>(
                e => e.Value == guid
            );
    }

    [Fact]
    public void ExpenseId_WhenPassedEmptyGuid_ShouldThrowInvalidExpenseIdException()
    {
        // Arrange
        var createExpenseId = () => new ExpenseId(Guid.Empty);

        // Act & Assert
        Assert.Throws<InvalidExpenseIdException>(createExpenseId);
    }
}