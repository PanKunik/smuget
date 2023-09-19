using System.Reflection.Metadata;
using Application.Exceptions;
using Application.MonthlyBillings.Commands.AddExpense;
using Application.Unit.Tests.MonthlyBillings.Commands.TestUtils;
using Application.Unit.Tests.TestUtils;
using Application.Unit.Tests.TestUtils.Constants;
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
        var addExpenseCommand = AddExpenseCommandUtils.CreateCommand();

        _repository
            .GetById(new(Constants.MonthlyBilling.Id))
            .Returns(
                MonthlyBillingUtils.CreateMonthlyBilling(
                    new List<Plan>() { PlanUtils.CreatePlan() }
                )
            );

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