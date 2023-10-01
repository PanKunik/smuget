using System;
using Domain.Exceptions;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.ValueObjects;

public sealed class IncomeIdTests
{
    [Fact]
    public void IncomeId_WhenPassedProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        Guid guid = Guid.NewGuid();

        // Act
        var result = new IncomeId(guid);

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .Match<IncomeId>(
                i => i.Value == guid
            );
    }

    [Fact]
    public void IncomeId_WhenPassedEmptyGuid_ShouldThrowInvalidIncomeIdException()
    {
        // Arrange
        var createIncomeId = () => new IncomeId(Guid.Empty);

        // Act & Assert
        Assert.Throws<InvalidIncomeIdException>(createIncomeId);
    }
}