using Application.Exceptions;
using Application.MonthlyBillings.Commands.CloseMonthlyBilling;
using Application.Unit.Tests.MonthlyBillings.Commands.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.Unit.Tests.MonthlyBillings.Commands.CloseMonthlyBilling;

public sealed class CloseMonthlyBillingCommandHandlerTests
{
    private readonly IMonthlyBillingRepository _repository;
    private readonly CloseMonthlyBillingCommandHandler _handler;

    public CloseMonthlyBillingCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingRepository>();
        _handler = new CloseMonthlyBillingCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingNotFound_ShouldThrowMonthlyBillingNotFoundException()
    {
        // Arrange
        var command = CloseMonthlyBillingCommandUtilities.CreateCommand();

        var closeMonthlyBilling = async () => await _handler.HandleAsync(
            command,
            default
        );

        // Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(closeMonthlyBilling);
    }

    [Fact]
    public async Task HandleAsync_WhenCloseMonthlyBillingIsValid_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var command = CloseMonthlyBillingCommandUtilities.CreateCommand();

        _repository
            .Get(
                new(command.Year),
                new(command.Month)
            )
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling());

        // Act
        await _handler.HandleAsync(
            command,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(
                Arg.Is<MonthlyBilling>(
                    m => m.Year == new Year(Constants.MonthlyBilling.Year)
                      && m.Month == new Month(Constants.MonthlyBilling.Month)
                )
            );
    }
}