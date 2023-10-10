using Application.Exceptions;
using Application.MonthlyBillings.AddExpense;
using Application.Unit.Tests.MonthlyBillings.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.Unit.Tests.MonthlyBillings.AddExpense;

public sealed class AddExpenseCommandHandlerTests
{
    private readonly IMonthlyBillingsRepository _repository;
    private readonly AddExpenseCommandHandler _handler;

    public AddExpenseCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingsRepository>();

        _repository
            .GetById(new(Constants.MonthlyBilling.Id))
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling());

        _handler = new AddExpenseCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_WhenInvoked_ShouldCallGetByIdOnRepositoryOnce()
    {
        // Arrange
        var addExpenseCommand = AddExpenseCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            addExpenseCommand,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .GetById(
                Arg.Is<MonthlyBillingId>(id => id.Value == addExpenseCommand.MonthlyBillingId)
            );
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingDoesntExist_ShouldThrowMonthlyBillingNotFoundException()
    {
        // Arrange
        var addExpenseCommand = new AddExpenseCommand(
            Guid.NewGuid(),
            Constants.Plan.Id,
            Constants.Expense.Money,
            Constants.Expense.Currency,
            Constants.Expense.ExpenseDate,
            Constants.Expense.Description
        );

        var addExpense = () => _handler.HandleAsync(
            addExpenseCommand,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(addExpense);
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingFoundAndExpenseCreatedSuccessfully_ShouldCallSaveOnRepositoryOnce()
    {
        // Arrange
        var addExpenseCommand = AddExpenseCommandUtilities.CreateCommand();

        // Act
        await _handler.HandleAsync(
            addExpenseCommand,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(Arg.Any<MonthlyBilling>());
    }

    [Fact]
    public async Task HandleAsync_OnSuccess_PassedArgumentShouldContainNewExpense()
    {
        // Arrange
        var addExpenseCommand = AddExpenseCommandUtilities.CreateCommand();

        MonthlyBilling passedMonthlyBilling = null;

        await _repository
            .Save(Arg.Do<MonthlyBilling>(m => passedMonthlyBilling = m));

        // Act
        await _handler.HandleAsync(
            addExpenseCommand,
            default
        );

        // Assert
        passedMonthlyBilling
            .Should()
            .NotBeNull();

        passedMonthlyBilling?.Plans
            .First().Expenses
                .Should()
                .ContainEquivalentOf(
                    new Expense(
                        new(Guid.NewGuid()),
                        new(
                            addExpenseCommand.Money,
                            new(addExpenseCommand.Currency)
                        ),
                        addExpenseCommand.ExpenseDate,
                        addExpenseCommand.Description
                    ),
                    c => c.Excluding(f => f.Id)
                );
    }
}