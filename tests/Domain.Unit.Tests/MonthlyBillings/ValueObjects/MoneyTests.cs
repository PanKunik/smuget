using Domain.Exceptions;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.ValueObjects;

public sealed class MoneyTests
{
    [Fact]
    public void Money_WhenPassedProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        var createMoney = () => new Money(
            15.06m,
            new Currency("PLN")
        );

        // Act
        var result = createMoney();

        // Assert
        result.Should().NotBeNull();
        result.Should().Match<Money>(
            m => m.Amount == 15.06m
            && m.Currency == new Currency("PLN")
        );
    }

    [Theory]
    [InlineData(10, 20, 30)]
    [InlineData(-10, 10, 0)]
    [InlineData(15, -5, 10)]
    public void AddOperator_WhenPassedTwoValues_ShouldAddThem(decimal a, decimal b, decimal expected)
    {
        // Arrange
        var left = new Money(a, new Currency("PLN"));
        var right = new Money(b, new Currency("PLN"));

        // Act
        var result = left + right;

        // Assert
        result.Should().Match<Money>(
            m => m.Amount == expected
            && m.Currency == left.Currency
            && m.Currency == right.Currency
        );
    }

    [Fact]
    public void AddOperator_WhenPassedTwoValuesWithDifferentCurrencies_ShoudlThrowMoneyCurrencyMismatchException()
    {
        // Arrange
        var left = new Money(15.06m, new Currency("PLN"));
        var right = new Money(4.95m, new Currency("EUR"));

        var addMoney = () => left + right;

        // Act & Assert
        Assert.Throws<MoneyCurrencyMismatchException>(addMoney);
    }

    [Theory]
    [InlineData(10, 10, 0)]
    [InlineData(10, -10, 20)]
    [InlineData(-10, 10, -20)]
    public void MinusOperator_WhenPassedTwoValues_ShouldSubstractThem(decimal a, decimal b, decimal expected)
    {
        // Arrange
        var left = new Money(a, new Currency("EUR"));
        var right = new Money(b, new Currency("EUR"));

        // Act
        var result = left - right;

        // Assert
        result.Should().Match<Money>(
            m => m.Amount == expected
            && m.Currency == left.Currency
            && m.Currency == right.Currency
        );
    }

    [Fact]
    public void MinusOperator_WhenPassedTwoValuesWithDifferentCurrencies_ShouldThrowMoneyCurrencyMismatchException()
    {
        // Arrange
        var left = new Money(-21.07m, new Currency("USD"));
        var right = new Money(13.85m, new Currency("EUR"));

        var substractMoney = () => left - right;

        // Act & Assert
        Assert.Throws<MoneyCurrencyMismatchException>(substractMoney);
    }

    [Fact]
    public void Equals_WhenPassedObjectWithSameValue_ShouldReturnTrue()
    {
        // Arrange
        var money = new Money(15.06m, new Currency("PLN"));
        var equalTo = new Money(15.06m, new Currency("PLN"));

        // Act
        var result = money.Equals(equalTo);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(15.06, "USD")]
    [InlineData(-21.04, "PLN")]
    public void Equals_WhenPassedObjectWithOtherValue_ShouldReturnFalse(decimal amount, string currency)
    {
        // Arrange
        var money = new Money(15.06m, new Currency("PLN"));
        var equalTo = new Money(amount, new Currency(currency));

        // Act
        var result = money.Equals(equalTo);

        // Assert
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(71.251, "PLN", "71,25 PLN")]
    [InlineData(98.3104, "USD", "98,31 USD")]
    [InlineData(184.96, "EUR", "184,96 EUR")]
    public void ToString_WhenCalled_ShouldReturnExpectedString(
        decimal amount,
        string currency,
        string expectedString
    )
    {
        // Arrange
        var cut = new Money(
            amount,
            new Currency(currency)
        );

        // Act
        var result = cut.ToString();

        // Assert
        result
            .Should()
            .BeEquivalentTo(expectedString);
    }
}