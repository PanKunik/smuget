using Application.Exceptions;
using Application.MonthlyBillings.OpenMonthlyBilling;
using Application.Unit.Tests.MonthlyBillings.TestUtilities;
using Application.Unit.Tests.TestUtilities;
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
        _handler = new OpenMonthlyBillingCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenOpenMonthlyBillingCommandIsValid_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var openMonthlyBillingCommand = OpenMonthlyBilingCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            openMonthlyBillingCommand,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(
                Arg.Is<MonthlyBilling>(
                    m => m.Currency == new Currency("PLN")
                      && m.Month == new Month(7)
                      && m.Year == new Year(2023)
                )
            );
    }

    [Fact]
    public async Task HandleAsync_WhenOpenMonthlyBillingExists_ShouldThrowMonthlyBillingAlreadyOpenedException()
    {
        // Arrange
        var openMonthlyBillingCommand = OpenMonthlyBilingCommandUtilities.CreateCommand();
        _repository
            .Get(
                Arg.Is<Year>(y => y.Value == 2023),
                Arg.Is<Month>(m => m.Value == 7)
            )
            .Returns(
                MonthlyBillingUtilities.CreateMonthlyBilling()
            );

        var openMonthlyBilling = async () => await _handler.HandleAsync(openMonthlyBillingCommand);

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingAlreadyOpenedException>(openMonthlyBilling);
    }
}