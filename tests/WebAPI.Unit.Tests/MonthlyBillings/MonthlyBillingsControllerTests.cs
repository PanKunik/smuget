using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.CQRS;
using Application.MonthlyBillings.CloseMonthlyBilling;
using Application.MonthlyBillings.OpenMonthlyBilling;
using Application.MonthlyBillings.ReopenMonthlyBilling;
using Application.MonthlyBillings;
using Application.MonthlyBillings.GetByYearAndMonth;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.MonthlyBillings;
using System;
using WebAPI.Services.Users;

namespace WebAPI.Unit.Tests.MonthlyBillings;

public sealed class MonthlyBillingsControllerTests
{
    private readonly MonthlyBillingsController _cut;

    private readonly Mock<ICommandHandler<OpenMonthlyBillingCommand>> _mockOpenMonthlyBillingCommandHandler;
    private readonly Mock<IQueryHandler<GetMonthlyBillingByYearAndMonthQuery, MonthlyBillingDTO>> _mockGetMonthlyBillingQueryHandler;
    private readonly Mock<ICommandHandler<CloseMonthlyBillingCommand>> _mockCloseMonthlyBillingCommandHandler;
    private readonly Mock<ICommandHandler<ReopenMonthlyBillingCommand>> _mockReopenMonthlyBillingCommandHandler;
    private readonly Mock<IUserService> _mockUserService;

    public MonthlyBillingsControllerTests()
    {
        _mockOpenMonthlyBillingCommandHandler = new Mock<ICommandHandler<OpenMonthlyBillingCommand>>();
        _mockGetMonthlyBillingQueryHandler = new Mock<IQueryHandler<GetMonthlyBillingByYearAndMonthQuery, MonthlyBillingDTO>>();
        _mockCloseMonthlyBillingCommandHandler = new Mock<ICommandHandler<CloseMonthlyBillingCommand>>();
        _mockReopenMonthlyBillingCommandHandler = new Mock<ICommandHandler<ReopenMonthlyBillingCommand>>();
        _mockUserService = new Mock<IUserService>();

        _mockUserService
            .Setup(m => m.UserId)
            .Returns(Guid.NewGuid());

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
            _mockGetMonthlyBillingQueryHandler.Object,
            _mockCloseMonthlyBillingCommandHandler.Object,
            _mockReopenMonthlyBillingCommandHandler.Object,
            _mockUserService.Object
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
    public async Task Get_OnSuccess_ShouldReturnOkObjectResult()
    {
        // Act
        var result = await _cut.Get(2023, 6);

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
        // Act
        var result = (OkObjectResult)await _cut.Get(2023, 6);

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
        // Act
        await _cut.Get(year, month);

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
        // Act
        var result = (OkObjectResult)await _cut.Get(2023, 1);

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
        // Act
        var result = await _cut.Close(2023, 1);

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
        // Act
        var result = (NoContentResult)await _cut.Close(2023, 1);

        // Assert
        result.StatusCode
            .Should()
            .Be(204);
    }

    [Fact]
    public async Task Close_WhenInvoked_ShouldCallCloseMonthlyBillingCommandHandler()
    {
        // Act
        await _cut.Close(2020, 1);

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
        await _cut.Close(year, month);

        // Assert
        _mockCloseMonthlyBillingCommandHandler.Verify(
            m => m.HandleAsync(
                It.Is<CloseMonthlyBillingCommand>(
                    c => c.Year == year
                      && c.Month == month),
                token),
            Times.Once);
    }

    [Fact]
    public async Task Reopen_OnSuccess_ShouldReturnNoContentResult()
    {
        // Act
        var result = await _cut.Reopen(2023, 1);

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Reopen_OnSuccess_ShouldReturn204StatusCode()
    {
        // Act
        var result = (NoContentResult)await _cut.Reopen(2023, 1);

        // Assert
        result.StatusCode
            .Should()
            .Be(204);
    }

    [Fact]
    public async Task Reopen_WhenInvoked_ShouldCallReopenMonthlyBillingCommandHandler()
    {
        // Act
        await _cut.Reopen(2020, 1);

        // Assert
        _mockReopenMonthlyBillingCommandHandler.Verify(
            m => m.HandleAsync(
                It.IsAny<ReopenMonthlyBillingCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData(2020, 1)]
    [InlineData(2021, 6)]
    [InlineData(2022, 12)]
    public async Task Reopen_WhenInvoked_ShouldPassParametersToCommand(
        ushort year,
        byte month
    )
    {
        // Arrange
        var token = new CancellationToken();

        // Act
        await _cut.Reopen(year, month);

        // Assert
        _mockReopenMonthlyBillingCommandHandler.Verify(
            m => m.HandleAsync(
                It.Is<ReopenMonthlyBillingCommand>(
                    c => c.Year == year
                      && c.Month == month),
                token),
            Times.Once);
    }
}