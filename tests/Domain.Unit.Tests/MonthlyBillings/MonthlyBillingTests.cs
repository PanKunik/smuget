using System;
using System.Collections.Generic;
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
              && m.State == State.Open
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

    [Fact]
    public void AddIncome_WhenPassedProperData_ShouldAddIncomeToMonthlyBilling()
    {
        // Arrange
        var cut = new MonthlyBilling(
            new Year(2007),
            new Month(9)
        );

        // Act
        cut.AddIncome(new Income(
            new Name("TEST"),
            new Money(10m, Currency.PLN),
            true
        ));

        // Assert
        cut.Incomes.Should().HaveCount(1);
        cut.Incomes.Should().BeEquivalentTo(
            new List<Income>()
            {
                new Income(
                    new Name("TEST"),
                    new Money(10m, Currency.PLN),
                    true
                )
            }.AsReadOnly(),
        c => c.Excluding(e => e.Id));
    }

    [Fact]
    public void AddIncome_WhenPassedNull_ShouldThrowIncomeIsNullException()
    {
        // Arrange
        var cut = new MonthlyBilling(
            new Year(2007),
            new Month(9)
        );

        var addIncome = () => cut.AddIncome(null);

        // Act & Assert
        Assert.Throws<IncomeIsNullException>(addIncome);
    }

    [Fact]
    public void AddIncome_WHenTryingToAddIncomeWithNotUniqueName_ShouldThrowIncomeNameNotUniqueException()
    {
        // Arrange
        var cut = new MonthlyBilling(
            new Year(2007),
            new Month(5)
        );

        cut.AddIncome(
            new Income(
                new Name("Wpłata"),
                new Money(
                    3500.75m,
                    Currency.USD
                ),
                true
        ));

        var addIncomeWithNotUniqueName = () => cut.AddIncome(
            new Income(
                new Name("Wpłata"),
                new Money(
                    1456.91M,
                    Currency.EUR
                ),
                true
        ));

        // Act & Assert
        Assert.Throws<IncomeNameNotUniqueException>(addIncomeWithNotUniqueName);
    }

    [Fact]
    public void Close_WhenCalled_ShouldChangeState()
    {
        // Arrange
        var cut = new MonthlyBilling(
            new Year(2007),
            new Month(3)
        );

        // Act
        cut.Close();

        // Assert
        cut.State.Should().Be(State.Closed);
    }

    [Fact]
    public void Close_WhenCalledForClosedMonthlyBilling_ShouldThrowMonthlyBillingAlreadyClosed()
    {
        // Arrange
        var cut = new MonthlyBilling(
            new Year(2007),
            new Month(3)
        );

        cut.Close();

        var close = () => cut.Close();

        // Act & Assert
        Assert.Throws<MonthlyBillingAlreadyClosedException>(close);
    }
}