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
              && m.SumOfIncome == 11.75m
              && m.SumOfIncomeAvailableForPlanning == 11.75m
              && m.SumOfPlan == 12.5m
              && m.SumOfExpenses == 137.01m
              && m.SavingsForecast == -0.75m
              && m.AccountBalance == -125.26m
        );
    }

    [Fact]
    public void MonthlyBilling_WhenPassedNullYear_ShouldThrowYearIsNullException()
    {
        // Arrange
        var createMonthlyBilling = ()
            => new MonthlyBilling(
                new MonthlyBillingId(Guid.NewGuid()),
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
                new MonthlyBillingId(Guid.NewGuid()),
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
                new MonthlyBillingId(Guid.NewGuid()),
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
            Constants.MonthlyBilling.Id,
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency,
            Constants.MonthlyBilling.State
        );

        // Act
        cut.AddIncome(new Income(
            new IncomeId(Guid.NewGuid()),
            new Name("TEST"),
            new Money(10m, new Currency("PLN")),
            true
        ));

        // Assert
        cut.Incomes
            .Should()
            .HaveCount(1);

        cut.Incomes
            .First()
            .Should()
            .BeEquivalentTo(
                new Income(
                    new IncomeId(Guid.NewGuid()),
                    new Name("TEST"),
                    new Money(10m, new Currency("PLN")),
                    true
                ),
                i => i.Excluding(i => i.Id)
            );
    }

    [Fact]
    public void AddIncome_WhenMonthlyBillingIsClosed_ShouldThrowMonthyBillingAlreadyClosedException()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling(
            incomes: new List<Income>()
        );
        cut.Close();

        var addIncome = () => cut.AddIncome(
            new Income(
                new IncomeId(Guid.NewGuid()),
                Constants.Income.Name,
                Constants.Income.Money,
                true
            )
        );

        // Act & Assert
        Assert.Throws<MonthlyBillingAlreadyClosedException>(addIncome);
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
            Constants.MonthlyBilling.Id,
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency,
            Constants.MonthlyBilling.State
        );

        var addIncomeWithOtherCurrency = () => cut.AddIncome(
            new Income(
                new IncomeId(Guid.NewGuid()),
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
    public void UpdateIncome_WhenPassedProperData_ShouldUpdateIncome()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling();
        var firstIncome = cut.Incomes.FirstOrDefault();

        // Act
        cut.UpdateIncome(
            firstIncome.Id,
            new("UpdatedName"),
            new(
                1.01m,
                new("USD")
            ),
            false
        );

        // Assert
        var result = cut.Incomes.First();

        result
            .Should()
            .Match<Income>(
                i => i.Name.Value == "UpdatedName"
                  && i.Money.Amount == 1.01m
                  && i.Money.Currency.Value == "USD"
                  && i.Include == false
            );
    }

    [Fact]
    public void UpdateIncome_WhenIncomeNotFound_ShouldThrowIncomeNotFoundException()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling();

        var updateIncome = () => cut.UpdateIncome(
            new(Guid.NewGuid()),
            Constants.Income.Name,
            Constants.Income.Money,
            false
        );

        // Act & Assert
        Assert.Throws<IncomeNotFoundException>(updateIncome);
    }

    [Fact]
    public void UpdateIncome_WhenMonthlyBillingIsClosed_ShouldThrowMonthlyBillingAlreadyClosedException()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling();
        cut.Close();

        var firstIncome = cut.Incomes.FirstOrDefault();

        var updateIncome = () => cut.UpdateIncome(
            firstIncome.Id,
            Constants.Income.Name,
            Constants.Income.Money,
            false
        );

        // Act & Assert
        Assert.Throws<MonthlyBillingAlreadyClosedException>(updateIncome);
    }

    [Fact]
    public void RemoveIncome_WhenPassedNullIncomeId_ShouldThrowIncomeIdIsNullException()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling();

        var removeIncome = () => cut.RemoveIncome(
            null
        );

        // Act & Assert
        Assert.Throws<IncomeIdIsNullException>(removeIncome);
    }

    [Fact]
    public void RemoveIncome_WhenMonthlyBillingIsClosed_ShouldThrowMonthlyBillingAlreadyClosedException()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling();
        cut.Close();

        var removeIncome = () => cut.RemoveIncome(
            new(Guid.NewGuid())
        );

        // Act & Assert
        Assert.Throws<MonthlyBillingAlreadyClosedException>(removeIncome);
    }

    [Fact]
    public void RemoveIncome_WhenIncomeDoesntExistInMonthlyBilling_ShouldThrowIncomeNotFoundException()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling();

        var removeIncome = () => cut.RemoveIncome(
            new(Guid.NewGuid())
        );

        // Act & Assert
        Assert.Throws<IncomeNotFoundException>(removeIncome);
    }

    [Fact]
    public void RemoveIncome_WhenPassedProperData_ShouldRemoveIncomeFromMonthlyBilling()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling(
            incomes: new List<Income>()
            {
                new Income(
                    Constants.Income.Id,
                    Constants.Income.Name,
                    Constants.Income.Money,
                    true
                )
            }
        );

        // Act
        cut.RemoveIncome(
            Constants.Income.Id
        );

        // Assert
        cut.Incomes
            .First().Active
                .Should()
                .BeFalse();
    }

    // TODO: Refactor
    [Theory]
    [InlineData(10, 25, 35)]
    [InlineData(12.03, 673.23, 685.26)]
    public void SumOfIncome_WhenCalled_ShouldReturnExpectedSum(decimal notIncludeIncome, decimal includeIncome, decimal expectedSumOfIncome)
    {
        // Arrange
        var cut = new MonthlyBilling(
            Constants.MonthlyBilling.Id,
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency,
            Constants.MonthlyBilling.State
        );

        // Act
        cut.AddIncome(new Income(
            new IncomeId(Guid.NewGuid()),
            new Name("TEST"),
            new Money(notIncludeIncome, new Currency("PLN")),
            false
        ));

        cut.AddIncome(new Income(
            new IncomeId(Guid.NewGuid()),
            new Name("TEST2"),
            new Money(includeIncome, new Currency("PLN")),
            true
        ));

        // Assert
        cut.SumOfIncome
            .Should()
            .Be(expectedSumOfIncome);
    }

    // TODO: Refactor
    [Theory]
    [InlineData(10, 25, 35)]
    [InlineData(1, 19.23, 20.23)]
    public void SumOfIncomeAvailableForPlanning_WhenCalled_ShouldReturnExpectedSum(decimal firstIncomeAmount, decimal secondIncomeAmount, decimal expectedSumOfIncomeAvailableForPlanning)
    {
        // Arrange
        var cut = new MonthlyBilling(
            Constants.MonthlyBilling.Id,
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency,
            Constants.MonthlyBilling.State
        );

        // Act
        cut.AddIncome(new Income(
            new IncomeId(Guid.NewGuid()),
            new Name("TEST"),
            new Money(firstIncomeAmount, new Currency("PLN")),
            true
        ));

        cut.AddIncome(new Income(
            new IncomeId(Guid.NewGuid()),
            new Name("TEST2"),
            new Money(secondIncomeAmount, new Currency("PLN")),
            true
        ));

        // Assert
        cut.SumOfIncomeAvailableForPlanning
            .Should()
            .Be(expectedSumOfIncomeAvailableForPlanning);
    }

    // TODO: Refactor
    [Fact]
    public void SumOfIncomeAvailableForPlanning_WhenCalled_ShouldReturnExpectedSumOfIncludedIncomes()
    {
        // Arrange
        var cut = new MonthlyBilling(
            Constants.MonthlyBilling.Id,
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency,
            Constants.MonthlyBilling.State
        );

        // Act
        cut.AddIncome(new Income(
            new IncomeId(Guid.NewGuid()),
            new Name("TEST"),
            new Money(24.24m, new Currency("PLN")),
            true
        ));

        cut.AddIncome(new Income(
            new IncomeId(Guid.NewGuid()),
            new Name("TEST2"),
            new Money(58.12m, new Currency("PLN")),
            true
        ));

        cut.AddIncome(new Income(
            new IncomeId(Guid.NewGuid()),
            new Name("TEST3"),
            new Money(19.74m, new Currency("PLN")),
            false
        ));

        // Assert
        cut.SumOfIncomeAvailableForPlanning
            .Should()
            .Be(82.36m);
    }

    [Fact]
    public void AddPlan_WhenPassedProperData_ShouldAddPlanToMonthlyBilling()
    {
        // Arrange
        var cut = new MonthlyBilling(
            Constants.MonthlyBilling.Id,
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency,
            Constants.MonthlyBilling.State,
            null,
            null
        );

        var plan = new Plan(
            new PlanId(Guid.NewGuid()),
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
                    new PlanId(Guid.NewGuid()),
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
            new PlanId(Guid.NewGuid()),
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
            Constants.MonthlyBilling.Id,
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency,
            Constants.MonthlyBilling.State
        );

        var addPlanWithOtherCurrency = () => cut.AddPlan(
            new Plan(
                new PlanId(Guid.NewGuid()),
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
    public void AddPlan_WhenMonthlyBillingIsClosed_ShouldThrowMonthyBillingAlreadyClosedException()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling(
            plans: new List<Plan>()
        );
        cut.Close();

        var addPlan = () => cut.AddPlan(
            new Plan(
                new PlanId(Guid.NewGuid()),
                Constants.Plan.Category,
                Constants.Plan.Money,
                1
            )
        );

        // Act & Assert
        Assert.Throws<MonthlyBillingAlreadyClosedException>(addPlan);
    }

    [Fact]
    public void SumOfPlans_WhenCalled_ShouldReturnExpectedValue()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling(
            plans: MonthlyBillingUtilities.CreatePlans(plansCount: 3)
        );

        // Act
        var result = cut.SumOfPlan;

        // Assert
        result
            .Should()
            .Be(75m);
    }

    [Fact]
    public void SumOfPlans_WhenPlansAreEmpty_ShouldReturnZero()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling(
            plans: MonthlyBillingUtilities.CreatePlans(plansCount: 0)
        );

        // Act
        var result = cut.SumOfPlan;

        // Assert
        result
            .Should()
            .Be(0m);
    }

    [Fact]
    public void SumOfExpenses_WhenCalled_ShouldReturnSumOfExpenses()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling(
            plans: MonthlyBillingUtilities.CreatePlans(plansCount: 3)
        );

        // Act
        var result = cut.SumOfExpenses;

        // Assert
        result
            .Should()
            .Be(411.03m);
    }

    [Fact]
    public void SumOfExpenses_WhenPlansAreEmpty_ShouldReturn0()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling(
            plans: new List<Plan>() { }
        );

        // Act
        var result = cut.SumOfExpenses;

        // Assert
        result
            .Should()
            .Be(0m);
    }

    [Fact]
    public void AccountBalance_WhenCalled_ShouldReturnExpectedValue()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling(
            MonthlyBillingUtilities.CreatePlans(),
            MonthlyBillingUtilities.CreateExpenses(),
            MonthlyBillingUtilities.CreateIncomes(5)
        );

        // Act
        var result = cut.AccountBalance;

        // Assert
        result
            .Should()
            .Be(39.24m);
    }

    [Fact]
    public void SavingsForecast_WhenCalled_ShouldReturnExpectedValue()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling(
            MonthlyBillingUtilities.CreatePlans(plansCount: 3),
            MonthlyBillingUtilities.CreateExpenses(),
            MonthlyBillingUtilities.CreateIncomes(5)
        );

        // Act
        var result = cut.SavingsForecast;

        // Assert
        result
            .Should()
            .Be(101.25m);
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
            new ExpenseId(Guid.NewGuid()),
            new Money(125.0M, new Currency("PLN")),
            new DateTimeOffset(new DateTime(2023, 3, 6)),
            "eBay"
        );

        var addExpense = () => cut.AddExpense(new PlanId(Guid.NewGuid()), expense);

        // Act & Assert
        Assert.Throws<PlanNotFoundException>(addExpense);
    }

    [Fact]
    public void AddExpense_WhenTryingToAddExpenseWithOtherCurrency_ShouldThrowMonthlyBillingCurrencyMismatchException()
    {
        // Arrange
        var cut = new MonthlyBilling(
            Constants.MonthlyBilling.Id,
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency,
            Constants.MonthlyBilling.State,
            plans: MonthlyBillingUtilities.CreatePlans()
        );

        var addExpenseWithOtherCurrency = () => cut.AddExpense(
            new PlanId(cut.Plans.First().Id.Value),
            new Expense(
                new ExpenseId(Guid.NewGuid()),
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
    public void AddExpense_WhenMonthlyBillingIsClosed_ShouldThrowMonthlyBillingAlreadyClosedException()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling(expenses: new List<Expense>());
        cut.Close();
        var firstPlanId = cut.Plans.First().Id;

        var addExpense = () => cut.AddExpense(
            firstPlanId,
            new Expense(
                new ExpenseId(Guid.NewGuid()),
                Constants.Expense.Money,
                Constants.Expense.ExpenseDate,
                Constants.Expense.Descripiton
            )
        );

        // Act & Assert
        Assert.Throws<MonthlyBillingAlreadyClosedException>(addExpense);
    }

    [Fact]
    public void Close_WhenCalled_ShouldChangeState()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling();

        // Act
        cut.Close();

        // Assert
        cut.State
            .Should()
            .Be(State.Closed);
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

    [Fact]
    public void Reopen_WhenCalled_ShouldChangeState()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling();
        cut.Close();

        // Act
        cut.Reopen();

        // Assert
        cut.State
            .Should()
            .Be(State.Open);
    }

    [Fact]
    public void Reopen_WhenCalledForOpenedMonthlyBilling_ShouldThrowMonthlyBillingAlreadyOpened()
    {
        // Arrange
        var cut = MonthlyBillingUtilities.CreateMonthlyBilling();

        var reopen = () => cut.Reopen();

        // Act & Assert
        Assert.Throws<MonthlyBillingAlreadyOpenedException>(reopen);
    }
}