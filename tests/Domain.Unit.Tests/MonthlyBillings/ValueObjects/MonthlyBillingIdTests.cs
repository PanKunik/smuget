using System;
using Domain.Exceptions;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.ValueObjects;

public sealed class MonthlyBillingIdTests
{
    [Fact]
    public void MonthlyBillingId_WhenPassedProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        var guid = Guid.NewGuid();

        // Act
        var result = new MonthlyBillingId(guid);

        // Assert
        result
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .Be(guid);
    }

    [Fact]
    public void MonthlyBillingId_WhenPassedEmptyGuid_ShouldThrowInvalidEntityIdException()
    {
        // Arrange
        var emptyGuid = Guid.Empty;
        var createMonthlyBillingId = () => new MonthlyBillingId(emptyGuid);

        // Act & Assert
        Assert.Throws<InvalidMonthlyBillingIdException>(createMonthlyBillingId);
    }

    [Fact]
    public void Equals_WhenPassedObjectWithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var guid = Guid.NewGuid();
        var compareTo = new MonthlyBillingId(guid);
        var cut = new MonthlyBillingId(guid);

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
        var compareTo = new MonthlyBillingId(Guid.NewGuid());
        var cut = new MonthlyBillingId(Guid.NewGuid());

        // Act
        var result = cut.Equals(compareTo);

        // Assert
        result
            .Should()
            .BeFalse();
    }
}