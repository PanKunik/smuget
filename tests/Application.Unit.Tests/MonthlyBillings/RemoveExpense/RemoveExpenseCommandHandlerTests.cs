using Application.Exceptions;
using Application.MonthlyBillings.RemoveExpense;
using Application.Unit.Tests.MonthlyBillings.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;
using Domain.Users;

namespace Application.Unit.Tests.MonthlyBillings.RemoveExpense;

public sealed class RemoveExpenseCommandHandlerTests
{
    private readonly IMonthlyBillingsRepository _repository;
    private readonly RemoveExpenseCommandHandler _handler;

    public RemoveExpenseCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingsRepository>();

        _repository
            .GetById(
                new(Constants.MonthlyBilling.Id),
                new(Constants.User.Id)
            )
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling());

        _handler = new RemoveExpenseCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var removeExpense = RemoveExpenseCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            removeExpense,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .GetById(
                Arg.Is<MonthlyBillingId>(m => m.Value == removeExpense.MonthlyBillingId),
                Arg.Is<UserId>(m => m.Value == removeExpense.UserId)
            );
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingDoesntExist_ShouldThrowMonthlyBillingNotFoundException()
    {
        // Arrange
        var removeExpense = new RemoveExpenseCommand(
            Guid.NewGuid(),
            Constants.Plan.Id,
            Constants.Expense.Id,
            Constants.User.Id
        );

        var removeAction = () => _handler.HandleAsync(
            removeExpense,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(removeAction);
    }

    [Fact]
    public async Task HandleAsync_WhenMonthyBillingFoundAndExpenseRemovedSuccessfully_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var removeExpense = RemoveExpenseCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            removeExpense,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(Arg.Any<MonthlyBilling>());
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_PassedArgumentShouldContainNotActiveExpense()
    {
        // Arrange
        var removeExpenseCommand = RemoveExpenseCommandUtilities.CreateCommand();

        MonthlyBilling passedArgument = null;

        await _repository
            .Save(Arg.Do<MonthlyBilling>(a => passedArgument = a));

        // Act
        await _handler.HandleAsync(
            removeExpenseCommand,
            default
        );

        // Assert
        passedArgument
            .Should()
            .NotBeNull();

        passedArgument?.Plans
            .First().Expenses
                .Should()
                .ContainSingle(x => x.Active == false);
    }
}