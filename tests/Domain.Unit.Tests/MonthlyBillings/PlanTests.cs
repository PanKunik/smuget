using System;
using System.Linq;
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
        Money money = new Money(525.88m, new Currency("PLN"));
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
            && p.Money.Equals(money)
            && p.SortOrder == sortOrder
        );
    }

    [Fact]
    public void Plan_WhenPassedNullCategory_ShouldThrowCategoryIsNullException()
    {
        // Arrange
        var createPlan = () => new Plan(
            null,
            new Money(21m, new Currency("USD")),
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

    [Fact]
    public void AddExpense_WhenCalledWithProperData_ShouldAddExpenseToPlan()
    {
        // Arrange
        var cut = new Plan(
            new Category("Groceries"),
            new Money(400M, new Currency("USD")),
            1
        );

        var expense = new Expense(
            new Money(150M, new Currency("PLN")),
            new DateTimeOffset(new DateTime(2020, 3, 4)),
            "Farmer's market"
        );

        // Act
        cut.AddExpense(expense);

        // Assert
        cut.Expenses
            .Should()
            .HaveCount(1);

        var firstExpense = cut.Expenses.First();

        firstExpense
            .Should()
            .Match<Expense>(
                e => e.Money == new Money(150M, new Currency("PLN"))
                && e.ExpenseDate == new DateTimeOffset(new DateTime(2020, 3, 4))
                && e.Descritpion == "Farmer's market"
            );
    }
}