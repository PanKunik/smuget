using Application.Exceptions;
using Application.MonthlyBillings.UpdatePlan;
using Application.Unit.Tests.MonthlyBillings.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.Unit.Tests.MonthlyBillings.UpdatePlan;

public sealed class UpdatePlanCommandHandlerTests
{
    private readonly IMonthlyBillingsRepository _repository;
    private readonly UpdatePlanCommandHandler _handler;

    public UpdatePlanCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingsRepository>();

        _repository
            .GetById(new(Constants.MonthlyBilling.Id))
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling());

        _handler = new UpdatePlanCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var command = UpdatePlanCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
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
        var updatePlan = new UpdatePlanCommand(
            Guid.NewGuid(),
            Constants.Plan.Id,
            Constants.Plan.Category,
            Constants.Plan.MoneyAmount,
            Constants.Plan.Currency,
            Constants.Plan.SortOrder
        );

        var updatePlanAction = () => _handler
            .HandleAsync(
                updatePlan,
                default
            );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(updatePlanAction);
    }

    [Fact]
    public async Task HandleAsync_WhenMotnhlyBillingFoundAndPlanUpdatedSuccessfully_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var updatePlan = UpdatePlanCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            updatePlan,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(Arg.Any<MonthlyBilling>());
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_PassedArgumentShouldContainUpdatedPlan()
    {
        // Arrange
        var updatePlan = UpdatePlanCommandUtilities.CreateCommand();

        MonthlyBilling passedArgument = null;

        await _repository
            .Save(Arg.Do<MonthlyBilling>(a => passedArgument = a));

        // Act
        await _handler.HandleAsync(
            updatePlan,
            default
        );

        // Assert
        passedArgument
            .Should()
            .NotBeNull();

        passedArgument?.Plans
            .Should()
            .ContainEquivalentOf(
                new Plan(
                    new(Guid.NewGuid()),
                    new(updatePlan.Category),
                    new(
                        updatePlan.MoneyAmount,
                        new(updatePlan.Currency)
                    ),
                    updatePlan.SortOrder
                ),
                c => c
                    .Excluding(f => f.Id)
                    .Excluding(f => f.Expenses)
                    .Excluding(f => f.SumOfExpenses)
            );
    }
}