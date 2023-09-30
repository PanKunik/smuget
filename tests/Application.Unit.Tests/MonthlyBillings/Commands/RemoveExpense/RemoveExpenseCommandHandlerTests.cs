using Application.Exceptions;
using Application.MonthlyBillings.Commands.RemoveExpense;
using Application.Unit.Tests.MonthlyBillings.Commands.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.Unit.Tests.MonthlyBillings.Commands.RemoveExpense;

public sealed class RemoveExpenseCommandHandlerTests
{
    private readonly IMonthlyBillingRepository _repository;
    private readonly RemoveExpenseCommandHandler _handler;

    public RemoveExpenseCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingRepository>();
        _handler = new RemoveExpenseCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var command = RemoveExpenseCommandUtilities.CreateCommand();

        _repository
            .GetById(
                new(command.MonthlyBillingId)
            )
            .Returns(
                MonthlyBillingUtilities.CreateMonthlyBilling(
                    plans: new List<Plan>()
                    {
                        PlansUtilities.CreatePlan(
                            expenses: new List<Expense>() { ExpenseUtilities.CreateExpense() }
                        )
                    }
                )
            );

        // Act
        await _handler.HandleAsync(
            command,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .GetById(
                new(command.MonthlyBillingId)
            );
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingDoesntExist_ShouldThrowMonthlyBillingNotFoundException()
    {
        // Arrange
        var command = RemoveExpenseCommandUtilities.CreateCommand();

        var removeExpense = async () => await _handler.HandleAsync(
            command,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(removeExpense);
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingFound_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var command = RemoveExpenseCommandUtilities.CreateCommand();
        var fakeMonthlyBilling = MonthlyBillingUtilities.CreateMonthlyBilling(
            plans: new List<Plan>() { PlansUtilities.CreatePlan(
                expenses: new List<Expense>() { ExpenseUtilities.CreateExpense() }
            ) }
        );

        _repository
            .GetById(
                new(command.MonthlyBillingId)
            )
            .Returns(
                fakeMonthlyBilling
            );

        // Act
        await _handler.HandleAsync(
            command,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(
                Arg.Is<MonthlyBilling>(
                    m => m.Plans.Any(p => p.Expenses.Any(e => e.Active == false))
                )
            );
    }
}