using Application.Exceptions;
using Application.MonthlyBillings.ReopenMonthlyBilling;
using Application.Unit.Tests.MonthlyBillings.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.Unit.Tests.MonthlyBillings.ReopenMonthlyBilling;

public sealed class ReopenMonthlyBillingCommandHandlerTests
{

    private readonly IMonthlyBillingRepository _repository;
    private readonly ReopenMonthlyBillingCommandHandler _handler;

    public ReopenMonthlyBillingCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingRepository>();

        var fakeMonthlyBilling = MonthlyBillingUtilities.CreateMonthlyBilling();
        fakeMonthlyBilling.Close();

        _repository
            .Get(
                new(Constants.MonthlyBilling.Year),
                new(Constants.MonthlyBilling.Month)
            )
            .Returns(fakeMonthlyBilling);

        _handler = new ReopenMonthlyBillingCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetOnRepositoryOnce()
    {
        // Arrange
        var reopen = ReopenMonthlyBillingCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            reopen,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Get(
                Arg.Is<Year>(y => y.Value == reopen.Year),
                Arg.Is<Month>(m => m.Value == reopen.Month)
            );
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingDoesntExist_ShouldThrowMonthlyBillingNotFoundException()
    {
        // Arrange
        var reopen = new ReopenMonthlyBillingCommand(
            2020,
            1
        );

        var reopenAction = () => _handler.HandleAsync(
            reopen,
            default
        );

        // Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(reopenAction);
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingFoundAndSuccessfullyReopened_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var reopen = ReopenMonthlyBillingCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            reopen,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(Arg.Any<MonthlyBilling>());
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_PassedArgumentShouldBeReopened()
    {
        // Arrange
        var reopen = ReopenMonthlyBillingCommandUtilities.CreateCommand();

        MonthlyBilling passedArgument = null;

        await _repository
            .Save(Arg.Do<MonthlyBilling>(m => passedArgument = m));

        // Act
        await _handler.HandleAsync(
            reopen,
            default
        );

        // Assert
        passedArgument
            .Should()
            .NotBeNull();

        passedArgument?.Year.Value
            .Should()
            .Be(reopen.Year);

        passedArgument?.Month.Value
            .Should()
            .Be(reopen.Month);

        passedArgument?.State
            .Should()
            .Be(State.Open);
    }
}