using Application.Exceptions;
using Application.MonthlyBillings.Commands.UpdatePlan;
using Application.Unit.Tests.MonthlyBillings.Commands.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.Unit.Tests.MonthlyBillings.Commands.UpdatePlan;

public sealed class UpdatePlanCommandHandlerTests
{
    private readonly IMonthlyBillingRepository _repository;
    private readonly UpdatePlanCommandHandler _handler;

    public UpdatePlanCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingRepository>();
        _handler = new UpdatePlanCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var command = UpdatePlanCommandUtilities.CreateCommand();
        _repository
            .GetById(new(command.MonthlyBillingId))
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling(
                plans: new List<Plan>() { PlansUtilities.CreatePlan() }
            ));

        // Act
        await _handler.HandleAsync(
            command,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .GetById(new(command.MonthlyBillingId));
    }

    [Fact]
    public async Task HandleAsync_WhenCalledForNotExistingMonthlyBilling_ShouldThrowMonthlyBillingNotFoundException()
    {
        // Arrange
        var command = UpdatePlanCommandUtilities.CreateCommand();

        var updatePlan = async () => await _handler
            .HandleAsync(
                command,
                default
            );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(updatePlan);
    }

    [Fact]
    public async Task HandleAsync_WhenPassedProperData_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var command = UpdatePlanCommandUtilities.CreateCommand();
        _repository
            .GetById(new(command.MonthlyBillingId))
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling(
                plans: new List<Plan>() { PlansUtilities.CreatePlan() }
            ));

        // Act
        await _handler.HandleAsync(
            command,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(Arg.Is<MonthlyBilling>(
                m => m.Plans.Any(
                    p => p.Id.Value == command.PlanId
                      && p.Category.Value == "Updated Category"
                      && p.Money.Amount == 258.96m
                      && p.Money.Currency.Value == "EUR"
                      && p.SortOrder == 99
                )
            ));
    }
}