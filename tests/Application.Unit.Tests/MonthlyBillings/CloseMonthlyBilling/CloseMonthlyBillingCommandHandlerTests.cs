using Application.Exceptions;
using Application.MonthlyBillings.CloseMonthlyBilling;
using Application.Unit.Tests.MonthlyBillings.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.Unit.Tests.MonthlyBillings.CloseMonthlyBilling;

public sealed class CloseMonthlyBillingCommandHandlerTests
{
    private readonly IMonthlyBillingsRepository _repository;
    private readonly CloseMonthlyBillingCommandHandler _handler;

    public CloseMonthlyBillingCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingsRepository>();

        _repository
            .Get(
                new(Constants.MonthlyBilling.Year),
                new(Constants.MonthlyBilling.Month)
            )
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling());

        _handler = new CloseMonthlyBillingCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetOnRepositoryOnce()
    {
        // Arrange
        var closeCommand = CloseMonthlyBillingCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            closeCommand,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Get(
                Arg.Is<Year>(y => y.Value == closeCommand.Year),
                Arg.Is<Month>(m => m.Value == closeCommand.Month)
            );
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingDoesntExist_ShouldThrowMonthlyBillingNotFoundException()
    {
        // Arrange
        var closeCommand = new CloseMonthlyBillingCommand(
            2000,
            1
        );

        var closeAction = () => _handler.HandleAsync(
            closeCommand,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(closeAction);
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingFoundAndClosedProperly_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var closeCommand = CloseMonthlyBillingCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            closeCommand,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(Arg.Any<MonthlyBilling>());
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_PassedArgumentShouldBeClosed()
    {
        // Arrange
        var closeCommand = CloseMonthlyBillingCommandUtilities.CreateCommand();

        MonthlyBilling passedArgument = null;

        await _repository
            .Save(Arg.Do<MonthlyBilling>(a => passedArgument = a));

        // Act
        await _handler.HandleAsync(
            closeCommand,
            default
        );

        // Assert
        passedArgument
            .Should()
            .NotBeNull();

        passedArgument?.State
            .Should()
            .BeEquivalentTo(State.Closed);
    }
}