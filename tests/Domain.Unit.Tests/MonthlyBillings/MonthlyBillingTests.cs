using System;
using Domain.Exceptions;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings;

public sealed class MonthlyBillingTests
{
    [Fact]
    public void MonthlyBilling_WhenPassedProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        Guid guid = Guid.NewGuid();
        var createMonthlyBilling = ()
            => new MonthlyBilling(
                    new Year(2000),
                    new Month(2)
                );

        // Act
        var result = createMonthlyBilling();

        // Assert
        result.Should().NotBeNull();
        result.Should().Match<MonthlyBilling>(
              m => m.Year == new Year(2000)
              && m.Month == new Month(2)
        );
    }

    [Fact]
    public void MonthlyBilling_WhenPassedNullYear_ShouldThrowYearIsNullException()
    {
        // Arrange
        var createMonthlyBilling = ()
            => new MonthlyBilling(
                null,
                new Month(3)
            );

        // Act & Assert
        Assert.Throws<YearIsNullException>(createMonthlyBilling);
    }

    [Fact]
    public void MonthlyBilling_WhenPassedNullMonth_ShouldThrowMonthIsNullException()
    {
        // Arrange
        var createMonthlyBilling = ()
            => new MonthlyBilling(
                new Year(2007),
                null
            );

        // Act & Assert
        Assert.Throws<MonthIsNullException>(createMonthlyBilling);
    }
}