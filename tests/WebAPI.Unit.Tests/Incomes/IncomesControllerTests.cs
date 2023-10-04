using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.CQRS;
using Application.MonthlyBillings.AddIncome;
using Application.MonthlyBillings.RemoveIncome;
using Application.MonthlyBillings.UpdateIncome;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Incomes;

namespace WebAPI.Unit.Tests.Incomes;

public sealed class IncomesControllerTests
{
    private readonly IncomesController _cut;

    private readonly Mock<ICommandHandler<AddIncomeCommand>> _mockAddIncomeCommandHandler;
    private readonly Mock<ICommandHandler<UpdateIncomeCommand>> _mockUpdateIncomeCommandHandler;
    private readonly Mock<ICommandHandler<RemoveIncomeCommand>> _mockRemoveIncomeCommandHandler;

    public IncomesControllerTests()
    {
        _mockAddIncomeCommandHandler = new Mock<ICommandHandler<AddIncomeCommand>>();
        _mockUpdateIncomeCommandHandler = new Mock<ICommandHandler<UpdateIncomeCommand>>();
        _mockRemoveIncomeCommandHandler = new Mock<ICommandHandler<RemoveIncomeCommand>>();

        _cut = new IncomesController(
            _mockAddIncomeCommandHandler.Object,
            _mockUpdateIncomeCommandHandler.Object,
            _mockRemoveIncomeCommandHandler.Object
        );
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
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<CreatedResult>();
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
        result.StatusCode
            .Should()
            .Be(201);
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
            Times.Once
        );
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
    public async Task UpdateIncome_OnSuccess_ShouldReturnNoContentResult()
    {
        // Act
        var result = await _cut.UpdateIncome(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new(
                "Updated name",
                11.24m,
                "USD",
                false
            )
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task UpdateIncome_OnSuccess_ShouldReturnStatusCode204()
    {
        // Act
        var result = (NoContentResult)await _cut.UpdateIncome(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new(
                "Updated name",
                11.24m,
                "USD",
                false
            )
        );

        // Assert
        result.StatusCode
            .Should()
            .Be(204);
    }

    [Fact]
    public async Task UpdateIncome_WhenInvoked_ShouldCallUpdateIncomeCommandHandler()
    {
        // Act
        await _cut.UpdateIncome(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new(
                "Updated name",
                11.24m,
                "USD",
                false
            )
        );

        // Assert
        _mockUpdateIncomeCommandHandler.Verify(
            m => m.HandleAsync(
                It.IsAny<UpdateIncomeCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task RemoveIncome_OnSuccess_ShouldReturnNoContentResult()
    {
        // Act
        var result = await _cut.RemoveIncome(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Assert
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task RemoveIncome_OnSuccess_ShouldReturn204StatusCode()
    {
        // Act
        var result = (NoContentResult)await _cut.RemoveIncome(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Assert
        result.StatusCode
            .Should()
            .Be(204);
    }

    [Fact]
    public async Task RemoveIncome_WhenInvoked_ShouldCallRemoveIncomeCommandHandler()
    {
        // Act
        await _cut.RemoveIncome(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Assert
        _mockRemoveIncomeCommandHandler.Verify(
            m => m.HandleAsync(
                It.IsAny<RemoveIncomeCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}