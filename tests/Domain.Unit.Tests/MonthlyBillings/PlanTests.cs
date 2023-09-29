using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.MonthlyBillings;
using Domain.Unit.Tests.MonthlyBillings.TestUtilities;

namespace Domain.Unit.Tests.MonthlyBillings;

public sealed class PlanTest
{
    [Fact]
    public void Plan_WhenPassedProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        PlanId id = new PlanId(Guid.NewGuid());
        Category category = new("Fuel");
        Money money = new Money(525.88m, new Currency("PLN"));
        uint sortOrder = 1u;

        var createPlan = () => new Plan(
            id,
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
    public void Plan_WhenPassedNullPlanId_ShouldThrowPlanIdIsNullException()
    {
        // Arrange
        var createPlan = () => new Plan(
            null,
            new Category("Category"),
            new Money(
                567.23M,
                new Currency("EUR")
            ),
            12
        );

        // Act & Assert
        Assert.Throws<PlanIdIsNullException>(createPlan);
    }

    [Fact]
    public void Plan_WhenPassedNullCategory_ShouldThrowCategoryIsNullException()
    {
        // Arrange
        var createPlan = () => new Plan(
            new PlanId(Guid.NewGuid()),
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
            new PlanId(Guid.NewGuid()),
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
            new PlanId(Guid.NewGuid()),
            new Category("Groceries"),
            new Money(400M, new Currency("USD")),
            1
        );

        var expense = new Expense(
            new ExpenseId(Guid.NewGuid()),
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
                && e.Description == "Farmer's market"
            );
    }

    [Fact]
    public void Update_WhenPassedNullCategory_ShouldThrowCategoryIsNullException()
    {
        // Arrange
        var cut = new Plan(
            Constants.Plan.Id,
            Constants.Plan.Category,
            Constants.Plan.Money,
            1
        );

        var updatePlan = () => cut.Update(
            null,
            new Money(
                123.45m,
                new Currency("USD")
            ),
            2
        );

        // Act & Assert
        Assert.Throws<CategoryIsNullException>(updatePlan);
    }

    [Fact]
    public void Update_WhenPassedNullMoney_ShouldThrowMoneyIsNullException()
    {
        // Arrange
        var cut = new Plan(
            Constants.Plan.Id,
            Constants.Plan.Category,
            Constants.Plan.Money,
            1
        );

        var updatePlan = () => cut.Update(
            new Category("Updated Category for plan"),
            null,
            5
        );

        // Act & Assert
        Assert.Throws<MoneyIsNullException>(updatePlan);
    }

    [Fact]
    public void Update_WhenPassedProperData_ShouldUpdatePlan()
    {
        // Arrange
        var cut = new Plan(
            Constants.Plan.Id,
            Constants.Plan.Category,
            Constants.Plan.Money,
            1
        );

        // Act
        cut.Update(
            new Category("Updated Category for plan"),
            new Money(
                123.45m,
                new Currency("USD")
            ),
            5
        );

        // Assert
        cut
            .Should()
            .Match<Plan>(
                p => p.Category.Value == "Updated Category for plan"
                  && p.Money.Amount == 123.45m
                  && p.Money.Currency.Value == "USD"
                  && p.SortOrder == 5
            );
    }

    [Fact]
    public void Remove_WhenCalled_ShouldSetActiveToFalse()
    {
        // Arrange
        var cut = new Plan(
            Constants.Plan.Id,
            Constants.Plan.Category,
            Constants.Plan.Money,
            1
        );

        // Act
        cut.Remove();

        // Assert
        cut.Active
            .Should()
            .BeFalse();
    }

    [Fact]
    public void SumOfExpense_WhenCalled_ShouldReturnExpectedValue()
    {
        // Arrange
        var cut = MonthlyBillingUtilities
            .CreatePlans()
            .FirstOrDefault();

        // Act
        var result = cut.SumOfExpenses;

        // Assert
        result
            .Should()
            .Be(137.01m);
    }

    [Fact]
    public void SumOfExpense_WhenCalledForEmptyExpenses_ShouldReturnZero()
    {
        // Arrange
        var cut = MonthlyBillingUtilities
            .CreatePlans(expenses: new List<Expense>() { })
            .FirstOrDefault();

        // Act
        var result = cut.SumOfExpenses;

        // Assert
        result
            .Should()
            .Be(0m);
    }
}