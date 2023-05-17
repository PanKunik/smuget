using Domain.Exceptions;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings;

public sealed class PlanTest
{
    [Fact]
    public void Plan_WhenPassedProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        Category category = new("Fuel");
        Money money = new Money(525.88m, Currency.PLN);
        uint sortOrder = 1u;

        var createPlan = () => new Plan(
            category,
            money,
            sortOrder
        );

        // Act
        var result = createPlan();

        // Assert
        result.Should().NotBeNull();
        result.Should().Match<Plan>(
            p => p.Category == category
            && p.MoneyAmount.Equals(money)
            && p.SortOrder == sortOrder
        );
    }

    [Fact]
    public void Plan_WhenPassedNullCategory_ShouldThrowCategoryIsNullException()
    {
        // Arrange
        var createPlan = () => new Plan(
            null,
            new Money(21m, Currency.USD),
            1u
        );

        // Act & Assert
        Assert.Throws<CategoryIsNullException>(createPlan);
    }

    [Fact]
    public void Plan_WhenPassedNullMoney_ShouldThrowMoneyIsNullException()
    {
        // Arrange
        var createPlan = () => new Plan(
            new Category("Shopping"),
            null,
            1u
        );

        // Act & Assert
        Assert.Throws<MoneyIsNullException>(createPlan);
    }
}