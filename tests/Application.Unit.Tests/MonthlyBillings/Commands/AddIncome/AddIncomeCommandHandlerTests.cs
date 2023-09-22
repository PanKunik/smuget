using Application.MonthlyBillings.Commands.AddIncome;
using Application.Unit.Tests.MonthlyBillings.Commands.TestUtilities;
using Domain.MonthlyBillings;
using Domain.Repositories;
using Application.Unit.Tests.TestUtilities.Constants;
using Application.Unit.Tests.TestUtilities;
using Application.Exceptions;

namespace Application.Unit.Tests.MonthlyBillings.Commands.AddIncome;

public sealed class AddIncomeCommandHandlerTests
{
    private readonly AddIncomeCommandHandler _handler;
    private readonly IMonthlyBillingRepository _repository;

    public AddIncomeCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingRepository>();

        _handler = new AddIncomeCommandHandler(_repository);
    }

    // TODO: Maybe extension method for long assertions?
    [Fact]
    public async Task HandleAsync_WhenAddIncomeCommandIsValid_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var addIncomeCommand = AddIncomeCommandUtilities.CreateCommand();

        _repository
            .GetById(new(Constants.MonthlyBilling.Id))
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling());

        // Act
        await _handler.HandleAsync(
            addIncomeCommand,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(
                Arg.Is<MonthlyBilling>(
                    m => m.Incomes.Any(
                        i => i.Name == new Name(Constants.Income.Name)
                          && i.Money == new Money(
                            Constants.Income.Amount,
                            new Currency(Constants.Income.Currency)
                          )
                          && i.Include == Constants.Income.Include
                    )
                )
            );
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingDoesntExist_ShouldThrowMonthlyBillingNotFoundException()
    {
        // Arrange
        var addIncomeCommand = AddIncomeCommandUtilities.CreateCommand();

        var addIncome = async () => await _handler.HandleAsync(addIncomeCommand);

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(addIncome);
    }
}