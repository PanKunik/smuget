using Application.Exceptions;
using Application.MonthlyBillings.Commands.AddExpense;
using Application.Unit.Tests.MonthlyBillings.Commands.TestUtils;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.Unit.Tests.MonthlyBillings.Commands.AddExpense;

public sealed class AddExpenseCommandHandlerTests
{
    private readonly IMonthlyBillingRepository _repository;
    private readonly AddExpenseCommandHandler _handler;

    public AddExpenseCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingRepository>();

        _handler = new AddExpenseCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenAddExpenseCommandIsValid_ShouldAddExpenseToPlanInMonthlyBilling()
    {
        // Arrange
        var monthlyBillingFake = MonthlyBillingUtilities.CreateMonthlyBilling(
            new List<Plan>()
            {
                new(
                    new(Guid.Parse("00000000-0000-0000-0000-000000000001")),
                    new(Constants.Plan.Category),
                    new(Constants.Plan.MoneyAmount, new(Constants.Plan.Currency)),
                    1,
                    MonthlyBillingUtilities.CreateExpenses(1)
                )
            }
        );

        var addExpenseCommand = AddExpenseCommandUtils.CreateCommand();

        _repository
            .GetById(new(Constants.MonthlyBilling.Id))
            .Returns(monthlyBillingFake);

        // Act
        await _handler.HandleAsync(
            addExpenseCommand,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(
                Arg.Is<MonthlyBilling>(
                    m => m.Plans.Any(
                        p => p.Expenses.Any(
                            e => e.Money == new Money(
                                Constants.Expense.Money,
                                new Currency(Constants.Expense.Currency)
                            )
                            && e.Description == Constants.Expense.Description
                            && e.ExpenseDate == Constants.Expense.ExpenseDate
                        )
                    )
                )
            );
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingNotFound_ShouldthrowMonthlyBillingNotFoundException()
    {
        // Arrange
        var addExpenseCommand = AddExpenseCommandUtils.CreateCommand();

        var addExpense = () => _handler.HandleAsync(
            addExpenseCommand,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(addExpense);
    }
}