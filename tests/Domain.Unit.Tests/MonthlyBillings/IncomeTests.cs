using Domain.Exceptions;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings;

public sealed class IncomeTests
{
    [Fact]
    public void Income_WhenPassedProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        Name name = new("Test name");
        Money money = new Money(525.88m, new Currency("PLN"));
        var include = true;

        var createIncome = () => new Income(
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
    public void Income_WhenPassedNullName_ShouldThrowNameIsNullException()
    {
        // Arrange
        var createIncome = () => new Income(
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
            new Name("TEST"),
            null,
            true
        );

        // Act & Assert
        Assert.Throws<MoneyIsNullException>(createIncome);
    }
}