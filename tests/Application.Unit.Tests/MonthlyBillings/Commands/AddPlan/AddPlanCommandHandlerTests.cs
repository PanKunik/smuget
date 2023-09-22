using Application.Exceptions;
using Application.MonthlyBillings.Commands.AddPlan;
using Application.Unit.Tests.MonthlyBillings.Commands.TestUtils;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.Unit.Tests.MonthlyBillings.Commands.AddPlan;

public sealed class AddPlanCommandHandlerTests
{
    private readonly IMonthlyBillingRepository _repository;
    private readonly AddPlanCommandHandler _handler;

    public AddPlanCommandHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingRepository>();

        _handler = new AddPlanCommandHandler(_repository);
    }

    [Fact]
    public async Task HandleAsync_AddPlanCommandIsValid_ShouldAddPlanToMonthlyBilling()
    {
        // Arrange
        var addPlanCommand = AddPlanCommandUtils.CreateCommand();

        _repository
            .GetById(new(Constants.MonthlyBilling.Id))
            .Returns(MonthlyBillingUtilities.CreateMonthlyBilling());

        // Act
        await _handler.HandleAsync(
            addPlanCommand,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Save(
                Arg.Is<MonthlyBilling>(
                    m => m.Plans.Any(
                        p => p.Category == new Category(Constants.Plan.Category)
                          && p.Money == new Money(
                            Constants.Plan.MoneyAmount,
                            new Currency(Constants.Plan.Currency)
                          )
                          && p.SortOrder == Constants.Plan.SortOrder
                    )
                )
            );
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingDoesntExist_ShouldThrowMonthlyBillingNotFoundException()
    {
        // Arrange
        var addPlanCommand = AddPlanCommandUtils.CreateCommand();

        var addPlan = () => _handler.HandleAsync(
            addPlanCommand,
            default
        );

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(addPlan);
    }
}