using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.CQRS;
using Application.MonthlyBillings.Commands.AddIncome;
using Application.MonthlyBillings.Commands.AddPlan;
using Application.MonthlyBillings.Commands.OpenMonthlyBilling;
using Domain.MonthlyBillings;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.MonthlyBillings;

namespace WebAPI.Unit.Tests.MonthlyBillings;

public sealed class MonthlyBillingsControllerTests
{
    private readonly MonthlyBillingsController _cut;

    private readonly Mock<ICommandHandler<OpenMonthlyBillingCommand>> _mockOpenMonthlyBillingCommandHandler;
    private readonly Mock<ICommandHandler<AddIncomeCommand>> _mockAddIncomeCommandHandler;
    private readonly Mock<ICommandHandler<AddPlanCommand>> _mockAddPlanCommandHandler;

    public MonthlyBillingsControllerTests()
    {
        _mockOpenMonthlyBillingCommandHandler = new Mock<ICommandHandler<OpenMonthlyBillingCommand>>();
        _mockAddIncomeCommandHandler = new Mock<ICommandHandler<AddIncomeCommand>>();
        _mockAddPlanCommandHandler = new Mock<ICommandHandler<AddPlanCommand>>();

        _cut = new MonthlyBillingsController(
            _mockOpenMonthlyBillingCommandHandler.Object,
            _mockAddIncomeCommandHandler.Object,
            _mockAddPlanCommandHandler.Object
        );
    }

    [Fact]
    public async Task Open_OnSuccess_ShouldReturnCreatedObjectResult()
    {
        // Act
        var result = await _cut.Open(new(2020, 1));

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CreatedResult>();
    }

    [Fact]
    public async Task Open_OnSuccess_ShouldReturnStatusCode201()
    {
        // Act
        var result = (CreatedResult)await _cut.Open(new(2020, 1));

        // Assert
        result.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task Open_WhenInvoked_ShouldCallOpenMonthlyBillingCommandHandler()
    {
        // Act
        await _cut.Open(new(2020, 1));

        // Assert
        _mockOpenMonthlyBillingCommandHandler.Verify(
            m => m.HandleAsync(
                It.IsAny<OpenMonthlyBillingCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData(2020, 1)]
    [InlineData(2021, 6)]
    [InlineData(2022, 12)]
    public async Task Open_WhenInvoked_ShouldPassParametersToCommand(ushort year, byte month)
    {
        // Arrange
        var token = new CancellationToken();

        // Act
        await _cut.Open(new(year, month));

        // Assert
        _mockOpenMonthlyBillingCommandHandler.Verify(
            m => m.HandleAsync(
                It.Is<OpenMonthlyBillingCommand>(
                    c => c.Year == year
                      && c.Month == month),
                token),
            Times.Once);
    }

    [Fact]
    public async Task AddIncome_OnSuccess_ShouldReturnCretedObjectResult()
    {
        // Arrange
        var request = new AddIncomeRequest(
            "TEST",
            5284M,
            Currency.PLN);

        // Act
        var result = await _cut.AddIncome(Guid.NewGuid(), request);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CreatedResult>();
    }

    [Fact]
    public async Task AddIncome_OnSuccess_ShouldReturnStatusCode201()
    {
        // Arrange
        var request = new AddIncomeRequest(
            "TEST",
            8761.97M,
            Currency.PLN);

        // Act
        var result = (CreatedResult)await _cut.AddIncome(Guid.NewGuid(), request);

        // Assert
        result.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task AddIncome_WhenInvoked_ShouldCallAddIncomeCommandHandler()
    {
        // Arrange
        var request = new AddIncomeRequest(
            "TEST",
            3456.20M,
            Currency.EUR);

        // Act
        await _cut.AddIncome(Guid.NewGuid(), request);

        // Assert
        _mockAddIncomeCommandHandler.Verify(
            a => a.HandleAsync(
                It.IsAny<AddIncomeCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData("2b715a6c-b187-4885-9344-c35f7f639f97", "TEST", 3680.70, Currency.PLN, true)]
    public async Task AddIncome_WhenInvoked_ShouldPassParametersToCommand(Guid monthlyBillingId, string name, decimal amount, Currency currency, bool include)
    {
        // Arrange
        var request = new AddIncomeRequest(
            name,
            amount,
            currency,
            include);

        var token = new CancellationToken();

        // Act
        await _cut.AddIncome(
            monthlyBillingId,
            request,
            token);

        // Assert
        _mockAddIncomeCommandHandler.Verify(
            a => a.HandleAsync(
                It.Is<AddIncomeCommand>(
                    a => a.MonthlyBillingId == monthlyBillingId
                    && a.Name == name
                    && a.Amount == amount
                    && a.Currency == currency
                    && a.Include == include
                ),
                token),
            Times.Once);
    }

    [Fact]
    public async Task AddPlan_OnSuccess_ShouldReturnCreatedResult()
    {
        // Act
        var result = await _cut.AddPlan(
            Guid.NewGuid(),
            new AddPlanRequest(
                "Shopping",
                100.0M,
                Currency.PLN,
                1
            )
        );

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CreatedResult>();
    }

    [Fact]
    public async Task AddPlan_OnSuccess_ShouldReturn201StatusCode()
    {
        // Act
        var result = (CreatedResult)await _cut.AddPlan(
            Guid.NewGuid(),
            new AddPlanRequest(
                "Shopping",
                100.0M,
                Currency.PLN,
                1
            ));

        // Assert
        result.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task AddPlan_WhenCalled_ShouldCallAddPlanCommandHandler()
    {
        // Arrange
        var addPlanRequest = new AddPlanRequest(
            "Food",
            10.0M,
            Currency.PLN,
            1
        );

        // Act
        await _cut.AddPlan(
            Guid.NewGuid(),
            addPlanRequest
        );

        // Assert
        _mockAddPlanCommandHandler.Verify(m => m.HandleAsync(
            It.IsAny<AddPlanCommand>(),
            default), Times.Once);
    }

    [Fact]
    public async Task AddPlan_WhenCalled_ShouldPassParametersToCommand()
    {
        // Arrange
        var addPlanRequest = new AddPlanRequest(
            "Fuel",
            87.96M,
            Currency.PLN,
            5
        );

        // Act
        await _cut.AddPlan(
            Guid.NewGuid(),
            addPlanRequest
        );

        // Assert
        _mockAddPlanCommandHandler.Verify(
            m => m.HandleAsync(
                It.Is<AddPlanCommand>(
                    c => c.Category == "Fuel"
                    && c.MoneyAmount == 87.96M
                    && c.Currency == Currency.PLN
                    && c.SortOrder == 5
                ),
                default
            ), Times.Once);
    }
}