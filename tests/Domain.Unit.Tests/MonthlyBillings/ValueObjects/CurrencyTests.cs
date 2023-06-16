using Domain.Exceptions;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.ValueObjects;

public sealed class CurrencyTests
{
    [Fact]
    public void Currency_WhenPassedValidData_ShouldReturnExpectedObject()
    {
        // Arrange
        var createCurrency = () => new Currency("PLN");

        // Act
        var result = createCurrency();

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .Match<Currency>(
                c => c.Value == "PLN"
            );
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Currency_WhenPassedNullOrWhiteSpaceString_ShouldThrowCurrencyIsNullException(string value)
    {
        // Arrange
        var createCurrency = () => new Currency(value);

        // Act & Assert
        Assert.Throws<CurrencyIsNullException>(createCurrency);
    }

    [Theory]
    [InlineData("ABC")]
    [InlineData("123")]
    [InlineData("AZB")]
    public void Currency_WhenPassedInvalidString_ShouldThrowInvalidCurrencyException(string value)
    {
        // Arrange
        var createCurrency = () => new Currency(value);

        // Act & Assert
        var exception = Assert.Throws<InvalidCurrencyException>(createCurrency);
        exception.Currency
            .Should()
            .Be(value);
    }

    [Theory]
    [InlineData("PLN", "PLN")]
    [InlineData("USD", "USD")]
    [InlineData("EUR", "EUR")]
    public void Equals_WhenCalledForObjectWithSameValue_ShouldReturnTrue(
        string value,
        string otherValue
    )
    {
        // Arrange
        var cut = new Currency(value);

        // Act
        var result = cut.Equals(new Currency(otherValue));

        // Assert
        result
            .Should()
            .BeTrue();
    }

    [Theory]
    [InlineData("PLN", "USD")]
    [InlineData("USD", "EUR")]
    [InlineData("EUR", "PLN")]
    public void Equals_WhenCalledForObjectWithOtherValue_ShouldReturnFalse(
        string value,
        string otherValue
    )
    {
        // Arrange
        var cut = new Currency(value);

        // Act
        var result = cut.Equals(new Currency(otherValue));

        // Assert
        result
            .Should()
            .BeFalse();
    }

    [Theory]
    [InlineData("PLN", "PLN")]
    [InlineData("USD", "USD")]
    [InlineData("EUR", "EUR")]
    public void ToString_WhenCalled_ShouldReturnExpectedString(
        string value,
        string expectedString
    )
    {
        // Arrange
        var cut = new Currency(value);

        // Act
        var result = cut.ToString();

        // Assert
        result
            .Should()
            .BeEquivalentTo(expectedString);
    }
}