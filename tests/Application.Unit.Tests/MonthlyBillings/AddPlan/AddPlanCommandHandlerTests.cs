using System.Reflection.Metadata;
using Application.Exceptions;
using Application.MonthlyBillings.AddPlan;
using Application.Unit.Tests.MonthlyBillings.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;
using NSubstitute.Routing.Handlers;

namespace Application.Unit.Tests.MonthlyBillings.AddPlan;

public sealed class AddPlanCommandHandlerTests
{
    private readonly IMonthlyBillingRepository _repository;
    private readonly AddPlanCommandHandler _handler;

    public AddPlanCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingRepository>();

        _repository
            .GetById(new(Constants.MonthlyBilling.Id))
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling());

        _handler = new AddPlanCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var addPlanCommand = AddPlanCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            addPlanCommand,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .GetById(Arg.Is<MonthlyBillingId>(
                m => m.Value == addPlanCommand.MonthlyBillingId
            ));
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingDoesntExist_ShouldThrowMonthlyBillingNotFoundException()
    {
        // Arrange
        var addPlanCommand = new AddPlanCommand(
            Guid.NewGuid(),
            Constants.Plan.Category,
            Constants.Plan.MoneyAmount,
            Constants.Plan.Currency,
            Constants.Plan.SortOrder
        );

        var addPlan = () => _handler.HandleAsync(
            addPlanCommand,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(addPlan);
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingFoundAndPlanCreatedSuccessfully_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var addPlanCommand = AddPlanCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            addPlanCommand,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(Arg.Any<MonthlyBilling>());
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_PassedArgumentShouldContainNewPlan()
    {
        // Arrange
        var addPlanCommand = AddPlanCommandUtilities.CreateCommand();

        MonthlyBilling passedArgument = null;
        await _repository.Save(Arg.Do<MonthlyBilling>(a => passedArgument = a));

        // Act
        await _handler.HandleAsync(
            addPlanCommand,
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
                    new(addPlanCommand.Category),
                    new(
                        addPlanCommand.MoneyAmount,
                        new(addPlanCommand.Currency)
                    ),
                    addPlanCommand.SortOrder
                ),
                c => c.Excluding(f => f.Id)
            );
    }
}