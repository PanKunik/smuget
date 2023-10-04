using Application.Exceptions;
using Application.MonthlyBillings.RemovePlan;
using Application.Unit.Tests.MonthlyBillings.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.Unit.Tests.MonthlyBillings.RemovePlan;

public sealed class RemovePlanCommandHandlerTests
{
    private readonly IMonthlyBillingRepository _repository;
    private readonly RemovePlanCommandHandler _handler;

    public RemovePlanCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingRepository>();
        _handler = new RemovePlanCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var command = RemovePlanCommandUtilities.CreateCommand();
        _repository
            .GetById(new(command.MonthlyBillingId))
            .Returns(
                MonthlyBillingUtilities.CreateMonthlyBilling(
                    plans: new List<Plan>()
                    {
                        PlansUtilities.CreatePlan()
                    }
                )
            );

        // Act
        await _handler
            .HandleAsync(
                command,
                default
            );

        // Assert
        await _repository
            .Received(1)
            .GetById(new(command.MonthlyBillingId));
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingDoesntExist_ShouldthrowMonthlyBillingNotFoundException()
    {
        // Arrange
        var command = RemovePlanCommandUtilities.CreateCommand();

        var removePlan = async () => await _handler
            .HandleAsync(
                command,
                default
            );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(removePlan);
    }

    [Fact]
    public async Task HandleAsync_WhenPassedProperData_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var command = RemovePlanCommandUtilities.CreateCommand();
        var fakeMonthlyBilling = MonthlyBillingUtilities.CreateMonthlyBilling(
            plans: new List<Plan>()
            {
                PlansUtilities.CreatePlan()
            }
        );
        _repository
            .GetById(new(command.MonthlyBillingId))
            .Returns(fakeMonthlyBilling);

        // Act
        await _handler
            .HandleAsync(
                command,
                default
            );

        // Assert
        await _repository
            .Received(1)
            .Save(Arg.Is<MonthlyBilling>(
                m => m.Plans.Any(i => !i.Active)
            ));
    }
}