using Application.MonthlyBillings.AddIncome;
using Application.Unit.Tests.MonthlyBillings.TestUtilities;
using Domain.MonthlyBillings;
using Domain.Repositories;
using Application.Unit.Tests.TestUtilities.Constants;
using Application.Unit.Tests.TestUtilities;
using Application.Exceptions;

namespace Application.Unit.Tests.MonthlyBillings.AddIncome;

public sealed class AddIncomeCommandHandlerTests
{
    private readonly AddIncomeCommandHandler _handler;
    private readonly IMonthlyBillingsRepository _repository;

    public AddIncomeCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingsRepository>();

        _repository
            .GetById(new(Constants.MonthlyBilling.Id))
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling());

        _handler = new AddIncomeCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var addIncomeCommand = AddIncomeCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            addIncomeCommand,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .GetById(
                Arg.Is<MonthlyBillingId>(id => id.Value == addIncomeCommand.MonthlyBillingId)
            );
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingDoesntExist_ShouldThrowMonthlyBillingNotFoundException()
    {
        // Arrange
        var addIncomeCommand = new AddIncomeCommand(
            Guid.NewGuid(),
            Constants.Income.Name,
            Constants.Income.Amount,
            Constants.Income.Currency,
            Constants.Income.Include
        );

        var addIncome = () => _handler.HandleAsync(
            addIncomeCommand,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(addIncome);
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingFoundAndIncomeCreatedSuccessfully_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var addIncomeCommand = AddIncomeCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            addIncomeCommand,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(Arg.Any<MonthlyBilling>());
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_PassedArgumentShouldContainNewIncome()
    {
        // Arrange
        var addIncomeCommand = AddIncomeCommandUtilities.CreateCommand();

        MonthlyBilling passedMonthlyBilling = null;

        await _repository
            .Save(Arg.Do<MonthlyBilling>(m => passedMonthlyBilling = m));

        // Act
        await _handler.HandleAsync(
            addIncomeCommand,
            default
        );

        // Assert
        passedMonthlyBilling
            .Should()
            .NotBeNull();

        passedMonthlyBilling?.Incomes
                .Should()
                .ContainEquivalentOf(
                    new Income(
                        new(Guid.NewGuid()),
                        new(addIncomeCommand.Name),
                        new(
                            addIncomeCommand.Amount,
                            new(addIncomeCommand.Currency)
                        ),
                        addIncomeCommand.Include
                    ),
                    c => c.Excluding(f => f.Id)
                );
    }
}