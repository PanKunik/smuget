using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.CQRS;
using Application.MonthlyBillings.Commands.AddExpense;
using Application.MonthlyBillings.Commands.AddIncome;
using Application.MonthlyBillings.Commands.AddPlan;
using Application.MonthlyBillings.Commands.CloseMonthlyBilling;
using Application.MonthlyBillings.Commands.OpenMonthlyBilling;
using Application.MonthlyBillings.DTO;
using Application.MonthlyBillings.Queries.GetByYearAndMonth;
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
    private readonly Mock<ICommandHandler<AddExpenseCommand>> _mockAddExpenseCommandHandler;
    private readonly Mock<IQueryHandler<GetMonthlyBillingByYearAndMonthQuery, MonthlyBillingDTO>> _mockGetMonthlyBillingQueryHandler;
    private readonly Mock<ICommandHandler<CloseMonthlyBillingCommand>> _mockCloseMonthlyBillingCommandHandler;

    public MonthlyBillingsControllerTests()
    {
        _mockOpenMonthlyBillingCommandHandler = new Mock<ICommandHandler<OpenMonthlyBillingCommand>>();
        _mockAddIncomeCommandHandler = new Mock<ICommandHandler<AddIncomeCommand>>();
        _mockAddPlanCommandHandler = new Mock<ICommandHandler<AddPlanCommand>>();
        _mockAddExpenseCommandHandler = new Mock<ICommandHandler<AddExpenseCommand>>();
        _mockGetMonthlyBillingQueryHandler = new Mock<IQueryHandler<GetMonthlyBillingByYearAndMonthQuery, MonthlyBillingDTO>>();
        _mockCloseMonthlyBillingCommandHandler = new Mock<ICommandHandler<CloseMonthlyBillingCommand>>();

        _mockGetMonthlyBillingQueryHandler
            .Setup(m => m.HandleAsync(
                It.Is<GetMonthlyBillingByYearAndMonthQuery>(
                    g => g.Year == 2023
                    && g.Month == 1
                ),
                It.IsAny<CancellationToken>()
            ))
            .ReturnsAsync(new MonthlyBillingDTO());

        _cut = new MonthlyBillingsController(
            _mockOpenMonthlyBillingCommandHandler.Object,
            _mockAddIncomeCommandHandler.Object,
            _mockAddPlanCommandHandler.Object,
            _mockAddExpenseCommandHandler.Object,
            _mockGetMonthlyBillingQueryHandler.Object,
            _mockCloseMonthlyBillingCommandHandler.Object
        );
    }

    [Fact]
    public async Task Open_OnSuccess_ShouldReturnCreatedObjectResult()
    {
        // Act
        var result = await _cut.Open(new(2020, 1, "USD"));

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<CreatedResult>();
    }

    [Fact]
    public async Task Open_OnSuccess_ShouldReturnStatusCode201()
    {
        // Act
        var result = (CreatedResult)await _cut.Open(new(2020, 1, "PLN"));

        // Assert
        result.StatusCode
            .Should()
            .Be(201);
    }

    [Fact]
    public async Task Open_WhenInvoked_ShouldCallOpenMonthlyBillingCommandHandler()
    {
        // Act
        await _cut.Open(new(2020, 1, "PLN"));

        // Assert
        _mockOpenMonthlyBillingCommandHandler.Verify(
            m => m.HandleAsync(
                It.IsAny<OpenMonthlyBillingCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData(2020, 1, "PLN")]
    [InlineData(2021, 6, "EUR")]
    [InlineData(2022, 12, "USD")]
    public async Task Open_WhenInvoked_ShouldPassParametersToCommand(
        ushort year,
        byte month,
        string currency
    )
    {
        // Arrange
        var token = new CancellationToken();

        // Act
        await _cut.Open(new(year, month, currency));

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
            "PLN"
        );

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
            "PLN"
        );

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
            "EUR");

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
    [InlineData("2b715a6c-b187-4885-9344-c35f7f639f97", "TEST", 3680.70, "PLN", true)]
    public async Task AddIncome_WhenInvoked_ShouldPassParametersToCommand(Guid monthlyBillingId, string name, decimal amount, string currency, bool include)
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
                "PLN",
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
                "PLN",
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
            "PLN",
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
            "PLN",
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
                    && c.Currency == "PLN"
                    && c.SortOrder == 5
                ),
                default
            ), Times.Once);
    }

    [Fact]
    public async Task AddExpense_OnSuccess_ShouldReturnCreatedResult()
    {
        // Arrange
        var request = new AddExpenseRequest(
            154.09M,
            "USD",
            new DateTimeOffset(new DateTime(2023, 4, 1)),
            "Description"
        );

        // Act
        var result = await _cut.AddExpense(
            Guid.NewGuid(),
            Guid.NewGuid(),
            request
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<CreatedResult>();
    }

    [Fact]
    public async Task AddExpense_OnSuccess_ShouldReturn201Created()
    {
        // Arrange
        var request = new AddExpenseRequest(
            154.09M,
            "USD",
            new DateTimeOffset(new DateTime(2023, 4, 1)),
            "Description"
        );

        // Act
        var result = (CreatedResult)await _cut.AddExpense(
            Guid.NewGuid(),
            Guid.NewGuid(),
            request
        );

        // Assert
        result.StatusCode
            .Should()
            .Be(201);
    }

    [Fact]
    public async Task AddExpense_WhenInvoked_ShouldCallAddExpenseCommandHandler()
    {
        // Arrange
        var request = new AddExpenseRequest(
            154.09M,
            "USD",
            new DateTimeOffset(new DateTime(2023, 4, 1)),
            "Description"
        );

        // Act
        await _cut.AddExpense(
            Guid.NewGuid(),
            Guid.NewGuid(),
            request
        );

        // Assert
        _mockAddExpenseCommandHandler.Verify(
            m => m.HandleAsync(
                It.IsAny<AddExpenseCommand>(),
                It.IsAny<CancellationToken>()
            ),
            Times.Once());
    }

    [Fact]
    public async Task AddExpense_WhenInvoked_ShouldPassParametersToCommandHandler()
    {
        // Arrange
        var request = new AddExpenseRequest(
            125.04M,
            "PLN",
            new DateTimeOffset(new DateTime(2023, 5, 1)),
            "TEST"
        );

        var monthlyBillingId = Guid.NewGuid();
        var planId = Guid.NewGuid();

        // Act
        await _cut.AddExpense(
            monthlyBillingId,
            planId,
            request
        );

        // Assert
        _mockAddExpenseCommandHandler.Verify(
            m => m.HandleAsync(
                It.Is<AddExpenseCommand>(
                    c => c.MonthlyBillingId == monthlyBillingId
                    && c.PlanId == planId
                    && c.Money == 125.04M
                    && c.Currency == "PLN"
                    && c.ExpenseDate == new DateTimeOffset(new DateTime(2023, 5, 1))
                    && c.Description == "TEST"
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once());
    }

    [Fact]
    public async Task Get_OnSuccess_ShouldReturnOkObjectResult()
    {
        // Arrange
        var request = new GetMonthlyBillingRequest(2023, 6);

        // Act
        var result = await _cut.Get(request);

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Get_OnSuccess_ShouldReturnStatusCode200Ok()
    {
        // Arrange
        var request = new GetMonthlyBillingRequest(2023, 6);

        // Act
        var result = (OkObjectResult)await _cut.Get(request);

        // Assert
        result.StatusCode
            .Should()
            .Be(200);
    }

    [Theory]
    [InlineData(2023, 5)]
    [InlineData(2022, 1)]
    [InlineData(2021, 12)]
    public async Task Get_WhenInvokedWithParameters_ShouldPassThemToQueryHandler(ushort year, byte month)
    {
        // Arrange
        var request = new GetMonthlyBillingRequest(
            year,
            month
        );

        // Act
        await _cut.Get(request);

        // Assert
        _mockGetMonthlyBillingQueryHandler.Verify(
            m => m.HandleAsync(
                It.Is<GetMonthlyBillingByYearAndMonthQuery>(
                    g => g.Year == year
                    && g.Month == month
                ),
                It.IsAny<CancellationToken>()
            ),
            Times.Once()
        );
    }

    [Fact]
    public async Task Get_OnSuccess_ShouldReturnExpectedObjectType()
    {
        // Arrange
        var request = new GetMonthlyBillingRequest(
            2023,
            1
        );

        // Act
        var result = (OkObjectResult)await _cut.Get(request);

        // Assert
        result.Value
            .Should()
            .NotBeNull();

        result.Value
            .Should()
            .BeOfType<MonthlyBillingDTO>();
    }

    [Fact]
    public async Task Close_OnSuccess_ShouldReturnNoContentResult()
    {
        // Arrange
        var request = new CloseMonthlyBillingRequest(
            2023,
            1
        );

        // Act
        var result = await _cut.Close(request);

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Close_OnSuccess_ShouldReturn204StatusCode()
    {
        // Arrange
        var request = new CloseMonthlyBillingRequest(
            2023,
            1
        );

        // Act
        var result = (NoContentResult)await _cut.Close(request);

        // Assert
        result.StatusCode
            .Should()
            .Be(204);
    }

    [Fact]
    public async Task Close_WhenInvoked_ShouldCallCloseMonthlyBillingCommandHandler()
    {
        // Act
        await _cut.Close(new(2020, 1));

        // Assert
        _mockCloseMonthlyBillingCommandHandler.Verify(
            m => m.HandleAsync(
                It.IsAny<CloseMonthlyBillingCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData(2020, 1)]
    [InlineData(2021, 6)]
    [InlineData(2022, 12)]
    public async Task Close_WhenInvoked_ShouldPassParametersToCommand(
        ushort year,
        byte month
    )
    {
        // Arrange
        var token = new CancellationToken();

        // Act
        await _cut.Close(new(year, month));

        // Assert
        _mockCloseMonthlyBillingCommandHandler.Verify(
            m => m.HandleAsync(
                It.Is<CloseMonthlyBillingCommand>(
                    c => c.Year == year
                      && c.Month == month),
                token),
            Times.Once);
    }
}