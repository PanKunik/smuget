using Application.Exceptions;
using Application.MonthlyBillings;
using Application.MonthlyBillings.GetByYearAndMonth;
using Application.Unit.Tests.MonthlyBillings.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;
using Domain.Users;

namespace Application.Unit.Tests.MonthlyBillings.GetByYearAndMonth;

public sealed class GetByYearAndMonthQueryHandlerTests
{
    private readonly IMonthlyBillingsRepository _repository;
    private readonly GetMonthlyBillingByYearAndMonthQueryHandler _handler;

    public GetByYearAndMonthQueryHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingsRepository>();

        _repository
            .Get(
                new(Constants.MonthlyBilling.Year),
                new(Constants.MonthlyBilling.Month),
                new(Constants.User.Id)
            )
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling(
                plans: new List<Plan>() { PlansUtilities.CreatePlan(
                            expenses: new List<Expense>() { ExpensesUtilities.CreateExpense() }
                        )
                    }
            ));

        _handler = new GetMonthlyBillingByYearAndMonthQueryHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetOnRepositoryOnce()
    {
        // Arrange
        var getQuery = GetMonthlyBillingByYearAndMonthQueryUtilities.CreateQuery();

        // Act
        await _handler.HandleAsync(
            getQuery,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Get(
                Arg.Is<Year>(y => y.Value == getQuery.Year),
                Arg.Is<Month>(m => m.Value == getQuery.Month),
                Arg.Is<UserId>(u => u.Value == getQuery.UserId)
            );
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingDoesntExist_ShouldThrowMonthlyBillingNotFoundException()
    {
        // Arrange
        var getQuery = new GetMonthlyBillingByYearAndMonthQuery(
            2020,
            1,
            Constants.User.Id
        );

        var getAction = () => _handler.HandleAsync(
            getQuery,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(getAction);
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingExists_ShouldReturnExpectedObject()
    {
        // Arrange
        var getQuery = GetMonthlyBillingByYearAndMonthQueryUtilities.CreateQuery();

        // Act
        var result = await _handler.HandleAsync(
            getQuery,
            default
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeEquivalentTo(ExpectedMonthlyBillingDTO());
    }

    private MonthlyBillingDTO ExpectedMonthlyBillingDTO()
        => new MonthlyBillingDTO()
        {
            Id = Constants.MonthlyBilling.Id,
            Year = Constants.MonthlyBilling.Year,
            Month = Constants.MonthlyBilling.Month,
            State = Constants.MonthlyBilling.State.Name,
            Incomes = ExpectedListOfIncomeDTOs(),
            Plans = ExpectedListOfPlanDTOs(),
            SavingsForecast = 1337.33m,
            SumOfExpenses = Constants.Expense.Money,
            SumOfIncome = Constants.Income.Amount,
            SumOfIncomeAvailableForPlanning = Constants.Income.Amount,
            SumOfPlan = Constants.Plan.MoneyAmount,
            AccountBalance = 3392.91m
        };

    private List<IncomeDTO> ExpectedListOfIncomeDTOs()
        => new List<IncomeDTO>()
        {
            new IncomeDTO()
            {
                Id = Constants.Income.Id,
                Name = Constants.Income.NameFromIndex(0).Value,
                Money = $"{ Constants.Income.Amount } { Constants.Income.Currency }",
                Include = Constants.Income.Include
            }
        };

    private List<PlanDTO> ExpectedListOfPlanDTOs()
        => new List<PlanDTO>()
        {
            new PlanDTO()
            {
                Id = Constants.Plan.Id,
                Category = Constants.Plan.Category,
                Money = $"{ Constants.Plan.MoneyAmount } { Constants.Plan.Currency }",
                SortOrder = Constants.Plan.SortOrder,
                Expenses = ExpectedListOfExpenseDTOs(),
                SumOfExpenses = Constants.Expense.Money
            }
        };

    private List<ExpenseDTO> ExpectedListOfExpenseDTOs()
        => new List<ExpenseDTO>()
        {
            new ExpenseDTO()
            {
                Id = Constants.Expense.Id,
                Money = $"{ Constants.Expense.Money } { Constants.Expense.Currency }",
                ExpenseDate = Constants.Expense.ExpenseDate,
                Description = Constants.Expense.Description
            }
        };
}