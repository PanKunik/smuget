using Application.Exceptions;
using Application.MonthlyBillings.RemoveIncome;
using Application.Unit.Tests.MonthlyBillings.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Application.Unit.Tests.TestUtilities;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.Unit.Tests.MonthlyBillings.RemoveIncome;

public sealed class RemoveIncomeCommandHandlerTests
{
    private readonly IMonthlyBillingsRepository _repository;
    private readonly RemoveIncomeCommandHandler _handler;

    public RemoveIncomeCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingsRepository>();

        _repository
            .GetById(new(Constants.MonthlyBilling.Id))
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling());

        _handler = new RemoveIncomeCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var command = RemoveIncomeCommandUtilities.CreateCommand();

        // Act
        await _handler
            .HandleAsync(
                command,
                default
            );

        // Assert
        await _repository
            .Received(1)
            .GetById(Arg.Is<MonthlyBillingId>(m => m.Value == command.MonthlyBillingId));
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingDoesntExist_ShouldThrowMonthlyBillingNotFoundException()
    {
        // Arrange
        var removeIncome = new RemoveIncomeCommand(
            Guid.NewGuid(),
            Constants.Income.Id
        );

        var removeIncomeAction = async () => await _handler
            .HandleAsync(
                removeIncome,
                default
            );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(removeIncomeAction);
    }

    [Fact]
    public async Task HandleAsync_WhenMonthyBillingFoundAndIncomeRemovedSuccessfully_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var removeIncome = RemoveIncomeCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            removeIncome,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(Arg.Any<MonthlyBilling>());
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_PassedArgumentShouldContainNotActiveIncome()
    {
        // Arrange
        var removeIncome = RemoveIncomeCommandUtilities.CreateCommand();

        MonthlyBilling passedArgument = null;

        await _repository
            .Save(Arg.Do<MonthlyBilling>(a => passedArgument = a));

        // Act
        await _handler.HandleAsync(
            removeIncome,
            default
        );

        // Assert
        passedArgument
            .Should()
            .NotBeNull();

        passedArgument?.Incomes
            .Should()
            .ContainSingle(x => x.Active == false);
    }
}