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
        _handler = new UpdateExpenseCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenCalled_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var command = UpdateExpenseCommandUtilities.CreateCommand();

        _repository
            .GetById(new(Constants.MonthlyBilling.Id))
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling(
                plans: new List<Plan>()
                {
                    PlansUtilities.CreatePlan(
                        new List<Expense>()
                        {
                            ExpenseUtilities.CreateExpense()
                        }
                    )
                }
            ));

        // Act
        await _handler.HandleAsync(
            command,
            default
        );

        // Act & Assert
        await _repository
            .Received(1)
            .GetById(new(command.MonthlyBillingId));
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingNotFound_ShouldThrowMonthlyBillingNotFound()
    {
        // Arrange
        var command = UpdateExpenseCommandUtilities.CreateCommand();

        var updateIncome = async () => await _handler.HandleAsync(
            command,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(updateIncome);
    }
}