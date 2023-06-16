using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Exceptions;
using Domain.MonthlyBillings;
using Domain.Unit.Tests.MonthlyBillings.TestUtilities;

namespace Domain.Unit.Tests.MonthlyBillings;

public sealed class MonthlyBillingTests
{
    [Fact]
    public void MonthlyBilling_WhenPassedProperData_ShouldReturnExpectedObject()
    {
        // Arrange
        Guid guid = Guid.NewGuid();
        var createMonthlyBilling = ()
            => MonthlyBillingUtilities.CreateMonthlyBilling();

        // Act
        var result = createMonthlyBilling();

        // Assert
        result.Should().NotBeNull();
        result.Should().Match<MonthlyBilling>(
              m => m.Year == new Year(2023)
              && m.Month == new Month(2)
              && m.Currency == new Currency("PLN")
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
                new Month(3),
                new Currency("PLN"),
                State.Open,
                null,
                null
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
                null,
                new Currency("PLN"),
                State.Open,
                null,
                null
            );

        // Act & Assert
        Assert.Throws<MonthIsNullException>(createMonthlyBilling);
    }

    [Fact]
    public void MonthlyBilling_WhenPassedNullCurrency_ShouldThrowCurrencyIsNullException()
    {
        // Arrange
        var createMonthlyBilling = ()
            => new MonthlyBilling(
                new Year(2007),
                new Month(3),
                null,
                State.Open,
                null,
                null
            );

        // Act & Assert
        Assert.Throws<CurrencyIsNullException>(createMonthlyBilling);
    }

    [Fact]
    public void AddIncome_WhenPassedProperData_ShouldAddIncomeToMonthlyBilling()
    {
        // Arrange
        var cut = new MonthlyBilling(
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency,
            Constants.MonthlyBilling.State
        );

        // Act
        cut.AddIncome(new Income(
            new Name("TEST"),
            new Money(10m, new Currency("PLN")),
            true
        ));

        // Assert
        cut.Incomes.Should().HaveCount(1);
        cut.Incomes.Should().BeEquivalentTo(
            new List<Income>()
            {
                new Income(
                    new Name("TEST"),
                    new Money(10m, new Currency("PLN")),
                    true
                )
            }.AsReadOnly(),
        c => c.Excluding(e => e.Id));
    }

    [Fact]
    public void AddIncome_WhenPassedNull_ShouldThrowIncomeIsNullException()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling();

        var addIncome = () => cut.AddIncome(null);

        // Act & Assert
        Assert.Throws<IncomeIsNullException>(addIncome);
    }

    [Fact]
    public void AddIncome_WhenTryingToAddIncomeWithNotUniqueName_ShouldThrowIncomeNameNotUniqueException()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling(
            incomes: MonthlyBillingUtilities.CreateIncomes()
        );

        var addIncomeWithNotUniqueName = () => cut.AddIncome(
            MonthlyBillingUtilities
                .CreateIncomes()
                .First()
            );

        // Act & Assert
        Assert.Throws<IncomeNameNotUniqueException>(addIncomeWithNotUniqueName);
    }

    [Fact]
    public void AddIncome_WhenTryingToAddIncomeWithOtherCurrency_ShouldThrowMonthlyBillingCurrencyMismatchException()
    {
        // Arrange
        var cut = new MonthlyBilling(
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency,
            Constants.MonthlyBilling.State
        );

        var addIncomeWithOtherCurrency = () => cut.AddIncome(
            new Income(
                Constants.Income.Name,
                new Money(
                    123.923M,
                    new Currency("EUR")
                ),
                true
            )
        );

        // Act & Assert
        Assert.Throws<MonthlyBillingCurrencyMismatchException>(addIncomeWithOtherCurrency);
    }

    [Fact]
    public void AddPlan_WhenPassedProperData_ShouldAddPlanToMonthlyBilling()
    {
        // Arrange
        var cut = new MonthlyBilling(
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency,
            Constants.MonthlyBilling.State,
            null,
            null
        );

        var plan = new Plan(
            new Category("Fuel"),
            new Money(156.84M, new Currency("PLN")),
            1
        );

        // Act
        cut.AddPlan(plan);

        // Assert
        cut.Plans.Should().HaveCount(1);
        cut.Plans.Should().BeEquivalentTo(
            new List<Plan>()
            {
                new Plan(
                    new Category("Fuel"),
                    new Money(156.84M, new Currency("PLN")),
                    1u
                )
            }.AsReadOnly(),
        c => c.Excluding(e => e.Id));
    }

    [Fact]
    public void AddPlan_WhenPlanIsNull_ShouldThrowPlanIsNullException()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling();

        Plan plan = null;

        var addPlan = () => cut.AddPlan(plan);

        // Act & Assert
        Assert.Throws<PlanIsNullException>(addPlan);
    }

    [Fact]
    public void AddPlan_WhenTryingToAddPlanWithSameCategory_ShouldThrowPlanCategoryNotUniqueExceptionn()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling();

        Plan plan = new Plan(
            new Category("Category 0"),
            new Money(
                123.45M,
                new Currency("PLN")
            ),
            1
        );

        var addPlan = () => cut.AddPlan(plan);

        // Act & Assert
        Assert.Throws<PlanCategoryNotUniqueException>(addPlan);
    }

    [Fact]
    public void AddPlan_WhenTryingToAddPlanWithOtherCurrency_ShouldThrowMonthlyBillingCurrencyMismatchException()
    {
        // Arrange
        var cut = new MonthlyBilling(
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency,
            Constants.MonthlyBilling.State
        );

        var addPlanWithOtherCurrency = () => cut.AddPlan(
            new Plan(
                Constants.Plan.Category,
                new Money(
                    842.10M,
                    new Currency("USD")
                ),
                1
            )
        );

        // Act & Assert
        Assert.Throws<MonthlyBillingCurrencyMismatchException>(addPlanWithOtherCurrency);
    }

    [Fact]
    public void AddExpense_WhenPassedNullPlanId_ShouldThrowPlanIdIsNullException()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling();

        PlanId planId = null;

        var addExpense = () => cut.AddExpense(planId, null);

        // Act & Assert
        Assert.Throws<PlanIdIsNullException>(addExpense);
    }

    [Fact]
    public void AddExpense_WhenPassedNullExpense_ShouldThrowExpenseIsNullException()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling();

        Expense expense = null;

        var addExpense = () => cut.AddExpense(new PlanId(Guid.NewGuid()), expense);

        // Act & Assert
        Assert.Throws<ExpenseIsNullException>(addExpense);
    }

    [Fact]
    public void AddExpense_WhenPlanNotFound_ShouldThrowPlanNotFoundException()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling();

        Expense expense = new Expense(
            new Money(125.0M, new Currency("PLN")),
            new DateTimeOffset(new DateTime(2023, 3, 6)),
            "eBay"
        );

        var addExpense = () => cut.AddExpense(new PlanId(Guid.NewGuid()), expense);

        // Act & Assert
        Assert.Throws<PlanNotFoundException>(addExpense);
    }

    [Fact(Skip = "Requires domain constructors refactor (id).")]
    public void AddExpense_WhenTryingToAddExpenseWithOtherCurrency_ShouldThrowMonthlyBillingCurrencyMismatchException()
    {
        // Arrange
        var cut = new MonthlyBilling(
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency,
            Constants.MonthlyBilling.State,
            plans: MonthlyBillingUtilities.CreatePlans()
        );

        var addExpenseWithOtherCurrency = () => cut.AddExpense(
            new PlanId(Guid.NewGuid()),
            new Expense(
                new Money(
                    65.99M,
                    new Currency("USD")
                ),
                Constants.Expense.ExpenseDate,
                Constants.Expense.Descripiton
            )
        );

        // Act & Assert
        Assert.Throws<MonthlyBillingCurrencyMismatchException>(addExpenseWithOtherCurrency);
    }

    [Fact]
    public void Close_WhenCalled_ShouldChangeState()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling();

        // Act
        cut.Close();

        // Assert
        cut.State.Should().Be(State.Closed);
    }

    [Fact]
    public void Close_WhenCalledForClosedMonthlyBilling_ShouldThrowMonthlyBillingAlreadyClosed()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling();

        cut.Close();

        var close = () => cut.Close();

        // Act & Assert
        Assert.Throws<MonthlyBillingAlreadyClosedException>(close);
    }
}