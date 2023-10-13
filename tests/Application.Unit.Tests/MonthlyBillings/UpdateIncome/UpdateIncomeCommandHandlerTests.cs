using Application.Exceptions;
using Application.MonthlyBillings.UpdateIncome;
using Application.Unit.Tests.MonthlyBillings.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;
using Domain.Users;

namespace Application.Unit.Tests.MonthlyBillings.UpdateIncome;

public sealed class UpdateIncomeCommandHandlerTests
{
    private readonly IMonthlyBillingsRepository _repository;
    private readonly UpdateIncomeCommandHandler _handler;

    public UpdateIncomeCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingsRepository>();

        _repository
            .GetById(
                new(Constants.MonthlyBilling.Id),
                new(Constants.User.Id)
            )
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling());

        _handler = new UpdateIncomeCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var updateIncome = UpdateIncomeCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            updateIncome,
            default
        );

        // Act & Assert
        await _repository
            .Received(1)
            .GetById(
                Arg.Is<MonthlyBillingId>(m => m.Value == updateIncome.MonthlyBillingId),
                Arg.Is<UserId>(u => u.Value == updateIncome.UserId)
            );
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingDoesntExist_ShouldThrowMonthlyBillingNotFound()
    {
        // Arrange
        var updateIncome = new UpdateIncomeCommand(
            Guid.NewGuid(),
            Constants.Income.Id,
            Constants.Income.Name,
            Constants.Income.Amount,
            Constants.Income.Currency,
            Constants.Income.Include,
            Constants.User.Id
        );

        var updateIncomeAction = () => _handler.HandleAsync(
            updateIncome,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(updateIncomeAction);
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingFoundAndIncomeUpdatedSuccessfully_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var updateIncome = UpdateIncomeCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            updateIncome,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(Arg.Any<MonthlyBilling>());
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_PassedArgumentShouldContainUpdatedIncome()
    {
        // Arrange
        var updateIncome = UpdateIncomeCommandUtilities.CreateCommand();

        MonthlyBilling passedArgument = null;

        await _repository
            .Save(Arg.Do<MonthlyBilling>(a => passedArgument = a));

        // Act
        await _handler.HandleAsync(
            updateIncome,
            default
        );

        // Assert
        passedArgument
            .Should()
            .NotBeNull();

        passedArgument?.Incomes
            .Should()
            .ContainEquivalentOf(
                new Income(
                    new(Guid.NewGuid()),
                    new(updateIncome.Name),
                    new(
                        updateIncome.MoneyAmount,
                        new(updateIncome.Currency)
                    ),
                    updateIncome.Include
                ),
                c => c.Excluding(f => f.Id)
            );
    }
}