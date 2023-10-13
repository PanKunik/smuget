using Application.Exceptions;
using Application.MonthlyBillings.RemovePlan;
using Application.Unit.Tests.MonthlyBillings.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;
using Domain.Users;

namespace Application.Unit.Tests.MonthlyBillings.RemovePlan;

public sealed class RemovePlanCommandHandlerTests
{
    private readonly IMonthlyBillingsRepository _repository;
    private readonly RemovePlanCommandHandler _handler;

    public RemovePlanCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingsRepository>();

        _repository
            .GetById(
                new(Constants.MonthlyBilling.Id),
                new(Constants.User.Id)
            )
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling());

        _handler = new RemovePlanCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var removePlan = RemovePlanCommandUtilities.CreateCommand();

        // Act
        await _handler
            .HandleAsync(
                removePlan,
                default
            );

        // Assert
        await _repository
            .Received(1)
            .GetById(
                Arg.Is<MonthlyBillingId>(m => m.Value == removePlan.MonthlyBillingId),
                Arg.Is<UserId>(u => u.Value == removePlan.UserId)
            );
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingDoesntExist_ShouldThrowMonthlyBillingNotFoundException()
    {
        // Arrange
        var removePlan = new RemovePlanCommand(
            Guid.NewGuid(),
            Constants.Plan.Id,
            Constants.User.Id
        );

        var removePlanAction = () => _handler
            .HandleAsync(
                removePlan,
                default
            );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(removePlanAction);
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingFoundAndRemovedPlanSuccessfully_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var removePlan = RemovePlanCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            removePlan,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(Arg.Any<MonthlyBilling>());
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_PassedArgumentShouldContainNotActivePlan()
    {
        // Arrange
        var removePlan = RemovePlanCommandUtilities.CreateCommand();

        MonthlyBilling passedArgument = null;

        await _repository
            .Save(Arg.Do<MonthlyBilling>(a => passedArgument = a));

        // Act
        await _handler.HandleAsync(
            removePlan,
            default
        );

        // Assert
        passedArgument
            .Should()
            .NotBeNull();

        passedArgument?.Plans
            .Should()
            .ContainSingle(x => x.Active == false);
    }
}