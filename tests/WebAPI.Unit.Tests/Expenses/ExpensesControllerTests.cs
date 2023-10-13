using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.CQRS;
using Application.MonthlyBillings.AddExpense;
using Application.MonthlyBillings.RemoveExpense;
using Application.MonthlyBillings.UpdateExpense;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Expenses;
using WebAPI.Services.Users;

namespace WebAPI.Unit.Tests.Expenses;

public sealed class ExpensesControllerTests
{
    private readonly ExpensesController _cut;

    private readonly Mock<ICommandHandler<AddExpenseCommand>> _mockAddExpenseCommandHandler;
    private readonly Mock<ICommandHandler<UpdateExpenseCommand>> _mockUpdateExpenseCommandHandler;
    private readonly Mock<ICommandHandler<RemoveExpenseCommand>> _mockRemoveExpenseCommandHandler;
    private readonly Mock<IUserService> _mockUserService;

    public ExpensesControllerTests()
    {
        _mockAddExpenseCommandHandler = new Mock<ICommandHandler<AddExpenseCommand>>();
        _mockUpdateExpenseCommandHandler = new Mock<ICommandHandler<UpdateExpenseCommand>>();
        _mockRemoveExpenseCommandHandler = new Mock<ICommandHandler<RemoveExpenseCommand>>();
        _mockUserService = new Mock<IUserService>();

        _mockUserService
            .Setup(m => m.UserId)
            .Returns(Guid.NewGuid());

        _cut = new ExpensesController(
            _mockAddExpenseCommandHandler.Object,
            _mockRemoveExpenseCommandHandler.Object,
            _mockUpdateExpenseCommandHandler.Object,
            _mockUserService.Object
        );
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
    public async Task UpdateExpense_OnSuccess_ShouldReturnNoContentResult()
    {
        // Act
        var result = await _cut.UpdateExpense(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new(
                11.24m,
                "USD",
                new DateTimeOffset(DateTime.Now),
                "Updated description"
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
    public async Task UpdateExpense_OnSuccess_ShouldReturnStatusCode204()
    {
        // Act
        var result = (NoContentResult)await _cut.UpdateExpense(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new(
                11.24m,
                "USD",
                new DateTimeOffset(DateTime.Now),
                "Updated description"
            )
        );

        // Assert
        result.StatusCode
            .Should()
            .Be(204);
    }

    [Fact]
    public async Task UpdateExpense_WhenInvoked_ShouldCallUpdateExpenseCommandHandler()
    {
        // Act
        await _cut.UpdateExpense(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            new(
                11.24m,
                "USD",
                new DateTimeOffset(DateTime.Now),
                "Updated description"
            )
        );

        // Assert
        _mockUpdateExpenseCommandHandler.Verify(
            m => m.HandleAsync(
                It.IsAny<UpdateExpenseCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task RemoveExpense_OnSuccess_ShouldReturnNoContentResult()
    {
        // Act
        var result = await _cut.RemoveExpense(
            Guid.NewGuid(),
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
    public async Task RemoveExpense_OnSuccess_ShouldReturnStatusCode204()
    {
        // Act
        var result = (NoContentResult)await _cut.RemoveExpense(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Assert
        result.StatusCode
            .Should()
            .Be(204);
    }

    [Fact]
    public async Task RemoveExpense_WhenInvoked_ShouldCallRemoveExpenseCommandHandler()
    {
        // Act
        await _cut.RemoveExpense(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Assert
        _mockRemoveExpenseCommandHandler.Verify(
            m => m.HandleAsync(
                It.IsAny<RemoveExpenseCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}