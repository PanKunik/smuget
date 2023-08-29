using System;
using Domain.Exceptions;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings;

public sealed class IncomeTests
{
    [Fact]
    public void Income_WhenPassedProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        IncomeId id = new(Guid.NewGuid());
        Name name = new("Test name");
        Money money = new(525.88m, new Currency("PLN"));
        const bool include = true;

        var createIncome = () => new Income(
            id,
            name,
            money,
            include
        );

        // Act
        var result = createIncome();

        // Assert
        result.Should().NotBeNull();
        result.Should().Match<Income>(
            i => i.Name == name
            && i.Money == money
            && i.Include == include
        );
    }

    [Fact]
    public void Income_WhenPassedNullIncomeId_ShouldThrowIncomeIdIsNullException()
    {
        // Arrange
        var createIncome = () => new Income(
            null,
            new("NAME"),
            new Money(
                91.53M,
                new Currency("PLN")
            ),
            true
        );

        // Act & Assert
        Assert.Throws<IncomeIdIsNullException>(createIncome);
    }

    [Fact]
    public void Income_WhenPassedNullName_ShouldThrowNameIsNullException()
    {
        // Arrange
        var createIncome = () => new Income(
            new IncomeId(Guid.NewGuid()),
            null,
            new Money(21m, new Currency("USD")),
            true
        );

        // Act & Assert
        Assert.Throws<NameIsNullException>(createIncome);
    }

    [Fact]
    public void Income_WhenPassedNullMoney_ShouldThrowMoneyIsNullException()
    {
        // Arrange
        var createIncome = () => new Income(
            new IncomeId(Guid.NewGuid()),
            new Name("TEST"),
            null,
            true
        );

        // Act & Assert
        Assert.Throws<MoneyIsNullException>(createIncome);
    }
}