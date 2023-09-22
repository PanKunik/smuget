using Application.Exceptions;
using Application.MonthlyBillings.DTO;
using Application.MonthlyBillings.Queries.GetByYearAndMonth;
using Application.Unit.Tests.MonthlyBillings.Queries.TestUtilities;
using Application.Unit.Tests.TestUtilities;
using Application.Unit.Tests.TestUtilities.Constants;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.Unit.Tests.MonthlyBillings.Queries.GetByYearAndMonth;

public sealed class GetByYearAndMonthQueryHandlerTests
{
    private readonly IMonthlyBillingRepository _repository;
    private readonly GetMonthlyBillingByYearAndMonthQueryHandler _handler;

    public GetByYearAndMonthQueryHandlerTests()
    {
        _repository = Substitute.For<IMonthlyBillingRepository>();

        _handler = new GetMonthlyBillingByYearAndMonthQueryHandler(_repository);
    }

    [Theory]
    [InlineData(2023, 9)]
    [InlineData(2022, 1)]
    public async Task HandleAsync_WhenCalled_ShouldCallGetOnRepository(int year, int month)
    {
        // Arrange
        var query = new GetMonthlyBillingByYearAndMonthQuery(
            (ushort)year,
            (byte)month
        );

        _repository
            .Get(
                new(year),
                new(month)
            )
            .Returns(
                new MonthlyBilling(
                    new(Constants.MonthlyBilling.Id),
                    new(year),
                    new(month),
                    new(Constants.MonthlyBilling.Currency),
                    Constants.MonthlyBilling.State,
                    null,
                    null
                )
            );

        // Act
        var result = await _handler.HandleAsync(
            query,
            default
        );

        // Assert
        await _repository
            .Received(1)
            .Get(
                Arg.Is<Year>(y => y.Value == year),
                Arg.Is<Month>(m => m.Value == month)
            );
    }

    [Fact]
    public async Task HandleAsync_WhenMonthlyBillingNotFound_ShouldReturnMonthlyBillingNotFoundException()
    {
        // Arrange
        var query = GetMonthlyBillingByYearAndMonthQueryUtilities.CreateQuery();
        var get = () => _handler.HandleAsync(query, default);

        // Act & Assert
        await Assert.ThrowsAsync<MonthlyBillingNotFoundException>(get);
    }

    [Fact]
    public async Task HandleAsync_WhenCalled_ShouldReturnExpectedObject()
    {
        // Arrange
        var query = GetMonthlyBillingByYearAndMonthQueryUtilities.CreateQuery();

        _repository
            .Get(
                new(Constants.MonthlyBilling.Year),
                new(Constants.MonthlyBilling.Month)
            )
            .Returns(
                MonthlyBillingUtilities.CreateMonthlyBilling(
                    MonthlyBillingUtilities.CreatePlans(
                        MonthlyBillingUtilities.CreateExpenses()
                    ),
                    MonthlyBillingUtilities.CreateIncomes()
                )
            );

        // Act
        var result = await _handler.HandleAsync(
            query,
            default
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .Match<MonthlyBillingDTO>(
                m => m.Year == Constants.MonthlyBilling.Year
                  && m.Month == Constants.MonthlyBilling.Month
                  && m.State == Constants.MonthlyBilling.State.Name
                  && m.SumOfIncome == 4321.45m
                  && m.SumOfPlan == 2984.12m
                  && m.SumOfExpenses == 2785.62m
                  && m.SavingsForecast == 1337.33m
                  && m.AccountBalance == 1535.83m
            );
    }
}