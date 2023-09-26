using Application.Exceptions;
using Application.MonthlyBillings.Commands.ReopenMonthlyBilling;
using Application.Unit.Tests.MonthlyBillings.Commands.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.Unit.Tests.MonthlyBillings.Commands.ReopenMonthlyBilling;

public sealed class ReopenMonthlyBillingCommandHandlerTests
{

    private readonly IMonthlyBillingRepository _repository;
    private readonly ReopenMonthlyBillingCommandHandler _handler;

    public ReopenMonthlyBillingCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingRepository>();
        _handler = new ReopenMonthlyBillingCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingNotFound_ShouldThrowMonthlyBillingNotFoundException()
    {
        // Arrange
        var command = ReopenMonthlyBillingCommandUtilities.CreateCommand();

        var reopenMonthlyBilling = async () => await _handler.HandleAsync(
            command,
            default
        );

        // Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(reopenMonthlyBilling);
    }

    [Fact]
    public async Task HandleAsync_WhenReopenMonthlyBillingIsValid_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var command = ReopenMonthlyBillingCommandUtilities.CreateCommand();
        var fakeMonthlyBilling = MonthlyBillingUtilities.CreateMonthlyBilling();
        fakeMonthlyBilling.Close();

        _repository
            .Get(
                new(command.Year),
                new(command.Month)
            )
            .Returns(fakeMonthlyBilling);

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
                      && m.State == State.Open
                )
            );
    }
}