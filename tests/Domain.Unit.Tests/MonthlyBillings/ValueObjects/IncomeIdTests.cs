using System;
using Domain.Exceptions;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.ValueObjects;

public sealed class IncomeIdTests
{
    [Fact]
    public void IncomeId_WhenPassedPRoperData_ShouldReturnExpectedObject()
    {
        // Arrange
        Guid guid = Guid.NewGuid();
        var createIncomeId = () => new IncomeId(guid);

        // Act
        var result = createIncomeId();

        // Assert
        result.Should().NotBeNull();
        result.Should().Match<IncomeId>(
            i => i.Value == guid);
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