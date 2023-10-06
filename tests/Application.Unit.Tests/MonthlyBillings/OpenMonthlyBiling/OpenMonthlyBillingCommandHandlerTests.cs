using Application.Exceptions;
using Application.MonthlyBillings.OpenMonthlyBilling;
using Application.Unit.Tests.MonthlyBillings.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.Unit.Tests.MonthlyBillings.OpenMonthlyBiling;

public sealed class OpenMonthlyBilingCommandHandlerTests
{
    private readonly OpenMonthlyBillingCommandHandler _handler;
    private readonly IMonthlyBillingRepository _repository;

    public OpenMonthlyBilingCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingRepository>();

        _repository
            .Get(
                new(Constants.MonthlyBilling.Year),
                new(Constants.MonthlyBilling.Month)
            )
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling());

        _handler = new OpenMonthlyBillingCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetOnRepositoryOnce()
    {
        // Arrange
        var openCommand = OpenMonthlyBilingCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            openCommand,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Get(
                new(openCommand.Year),
                new(openCommand.Month)
            );
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingForPassedYearAndMonthExist_ShouldThrowMonthlyBillingAlreadyOpenedException()
    {
        // Arrange
        var openCommand = new OpenMonthlyBillingCommand(
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency
        );

        var openAction = () => _handler.HandleAsync(
            openCommand,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingAlreadyOpenedException>(openAction);
    }

    [Fact]
    public async Task HandleAsync_MonthlyBillingSuccessfullyOpened_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var openCommand = OpenMonthlyBilingCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            openCommand,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(Arg.Any<MonthlyBilling>());
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_PassedArgumentShouldBeOpened()
    {
        // Arrange
        MonthlyBilling passedArgument = null;

        await _repository
            .Save(Arg.Do<MonthlyBilling>(m => passedArgument = m));

        var openCommand = OpenMonthlyBilingCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            openCommand,
            default
        );
        
        // Assert
        passedArgument
            .Should()
            .NotBeNull();

        passedArgument?.State
            .Should()
            .Be(State.Open);
    }
}