using System;
using Domain.Exceptions;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.ValueObjects;

public sealed class PlanIdTests
{
    [Fact]
    public void PlanId_WhenPassedProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        Guid guid = Guid.NewGuid();
        var createPlanId = () => new PlanId(guid);

        // Act
        var result = createPlanId();

        // Assert
        result.Should().NotBeNull();
        result.Should().Match<PlanId>(
            i => i.Value == guid);
    }

    [Fact]
    public void PlanId_WhenPassedEmptyGuid_ShouldThrowInvalidPlanIdException()
    {
        // Arrange
        var createPlanId = () => new PlanId(Guid.Empty);

        // Act & Assert
        Assert.Throws<InvalidPlanIdException>(createPlanId);
    }
}