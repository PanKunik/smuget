using Application.Exceptions;
using Application.MonthlyBillings.UpdateExpense;
using Application.Unit.Tests.MonthlyBillings.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.Unit.Tests.MonthlyBillings.UpdateExpense;

public sealed class UpdateExpenseCommandHandlerTests
{
    private readonly IMonthlyBillingRepository _repository;
    private readonly UpdateExpenseCommandHandler _handler;

    public UpdateExpenseCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingRepository>();

        _repository
            .GetById(new(Constants.MonthlyBilling.Id))
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling());

        _handler = new UpdateExpenseCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var command = UpdateExpenseCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            command,
            default
        );

        // Act & Assert
        await _repository
            .Received(1)
            .GetById(Arg.Is<MonthlyBillingId>(m => m.Value == command.MonthlyBillingId));
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingNotFound_ShouldThrowMonthlyBillingNotFound()
    {
        // Arrange
        var updateExpense = new UpdateExpenseCommand(
            Guid.NewGuid(),
            Constants.Plan.Id,
            Constants.Expense.Id,
            Constants.Expense.Money,
            Constants.Expense.Currency,
            Constants.Expense.ExpenseDate,
            Constants.Expense.Description
        );

        var updateExpenseAction = () => _handler.HandleAsync(
            updateExpense,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(updateExpenseAction);
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingFoundAndSuccessfullyUdpatedExpense_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var updateExpense = UpdateExpenseCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            updateExpense,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(Arg.Any<MonthlyBilling>());
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_PassedArgumentShouldContainUpdatedExpense()
    {
        // Arrange
        var updateExpense = UpdateExpenseCommandUtilities.CreateCommand();

        MonthlyBilling passedArgument = null;

        await _repository
            .Save(Arg.Do<MonthlyBilling>(a => passedArgument = a));

        // Act
        await _handler.HandleAsync(
            updateExpense,
            default
        );

        // Assert
        passedArgument
            .Should()
            .NotBeNull();

        passedArgument?.Plans
            .First().Expenses
                .Should()
                .ContainEquivalentOf(
                    new Expense(
                        new(Guid.NewGuid()),
                        new(
                            updateExpense.MoneyAmount,
                            new(updateExpense.Currency)
                        ),
                        updateExpense.ExpenseDate,
                        updateExpense.Description
                    ),
                    c => c.Excluding(f => f.Id)
                );
    }
}