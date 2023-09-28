using Application.Exceptions;
using Application.MonthlyBillings.Commands.RemoveIncome;
using Application.Unit.Tests.MonthlyBillings.Commands.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.Unit.Tests.MonthlyBillings.Commands.RemoveIncome;

public sealed class RemoveIncomeCommandHandlerTests
{
    private readonly IMonthlyBillingRepository _repository;
    private readonly RemoveIncomeCommandHandler _handler;

    public RemoveIncomeCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingRepository>();
        _handler = new RemoveIncomeCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var command = RemoveIncomeCommandUtilities.CreateCommand();
        _repository
            .GetById(new(command.MonthlyBillingId))
            .Returns(
                MonthlyBillingUtilities.CreateMonthlyBilling(
                    incomes: new List<Income>()
                    {
                        IncomesUtilities.CreateIncome()
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
        var command = RemoveIncomeCommandUtilities.CreateCommand();

        var removeIncome = async () => await _handler
            .HandleAsync(
                command,
                default
            );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(removeIncome);
    }

    [Fact]
    public async Task HandleAsync_WhenPassedProperData_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var command = RemoveIncomeCommandUtilities.CreateCommand();
        var fakeMonthlyBilling = MonthlyBillingUtilities.CreateMonthlyBilling(
            incomes: new List<Income>()
            {
                IncomesUtilities.CreateIncome()
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
                m => m.Incomes.Any(i => !i.Active)
            ));
    }
}