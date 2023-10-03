using Application.Exceptions;
using Application.MonthlyBillings.UpdateIncome;
using Application.Unit.Tests.MonthlyBillings.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.Unit.Tests.MonthlyBillings.UpdateIncome;

public sealed class UpdateIncomeCommandHandlerTests
{
    private readonly IMonthlyBillingRepository _repository;
    private readonly UpdateIncomeCommandHandler _handler;

    public UpdateIncomeCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingRepository>();
        _handler = new UpdateIncomeCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenCalled_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var command = UpdateIncomeCommandUtilities.CreateCommand();

        _repository
            .GetById(new(Constants.MonthlyBilling.Id))
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling(
                incomes: new List<Income>() { IncomesUtilities.CreateIncome() }
            ));

        // Act
        await _handler.HandleAsync(
            command,
            default
        );

        // Act & Assert
        await _repository
            .Received(1)
            .GetById(new(command.MonthlyBillingId));
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingNotFound_ShouldThrowMonthlyBillingNotFound()
    {
        // Arrange
        var command = UpdateIncomeCommandUtilities.CreateCommand();

        var updateIncome = async () => await _handler.HandleAsync(
            command,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(updateIncome);
    }

    [Fact]
    public async Task HandleAsync_WhenUpdateIncomeCommandIsValid_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var command = UpdateIncomeCommandUtilities.CreateCommand();

        _repository
            .GetById(new(Constants.MonthlyBilling.Id))
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling(
                incomes: new List<Income>() { IncomesUtilities.CreateIncome() }
            ));

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
                    m => m.Incomes.FirstOrDefault(
                        i => i.Name.Value == "Updated Name Income"
                          && i.Money.Amount == 1234.56m
                          && i.Money.Currency.Value == "EUR"
                          && i.Include == false) != null
                )
            );
    }
}