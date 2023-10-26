using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.CQRS;
using Application.MonthlyBillings.AddExpense;
using Application.MonthlyBillings.RemoveExpense;
using Application.MonthlyBillings.UpdateExpense;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WebAPI.Expenses;
using WebAPI.Services.Users;

namespace WebAPI.Unit.Tests.Expenses;

public sealed class ExpensesControllerTests
{
    private readonly ExpensesController _cut;

    private readonly ICommandHandler<AddExpenseCommand> _mockAddExpenseCommandHandler;
    private readonly ICommandHandler<UpdateExpenseCommand> _mockUpdateExpenseCommandHandler;
    private readonly ICommandHandler<RemoveExpenseCommand> _mockRemoveExpenseCommandHandler;
    private readonly IUserService _mockUserService;

    public ExpensesControllerTests()
    {
        _mockAddExpenseCommandHandler = Substitute.For<ICommandHandler<AddExpenseCommand>>();
        _mockUpdateExpenseCommandHandler = Substitute.For<ICommandHandler<UpdateExpenseCommand>>();
        _mockRemoveExpenseCommandHandler = Substitute.For<ICommandHandler<RemoveExpenseCommand>>();
        _mockUserService = Substitute.For<IUserService>();

        _mockUserService
            .UserId
            .Returns(Guid.NewGuid());

        _cut = new ExpensesController(
            _mockAddExpenseCommandHandler,
            _mockRemoveExpenseCommandHandler,
            _mockUpdateExpenseCommandHandler,
            _mockUserService
        );
    }

    [Fact]
    public async Task AddExpense_OnSuccess_ShouldReturnCreatedResult()
    {
        // Arrange
        var request = new AddExpenseRequest(
            154.09M,
            "USD",
            new DateOnly(2023, 4, 1),
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
            new DateOnly(2023, 4, 1),
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
            new DateOnly(2023, 4, 1),
            "Description"
        );

        // Act
        await _cut.AddExpense(
            Guid.NewGuid(),
            Guid.NewGuid(),
            request
        );

        // Assert
        await _mockAddExpenseCommandHandler
            .Received(1)
            .HandleAsync(
                Arg.Any<AddExpenseCommand>(),
                Arg.Any<CancellationToken>()
            );
    }

    [Fact]
    public async Task AddExpense_WhenInvoked_ShouldPassParametersToCommandHandler()
    {
        // Arrange
        var request = new AddExpenseRequest(
            125.04M,
            "PLN",
            new DateOnly(2023, 5, 1),
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
        await _mockAddExpenseCommandHandler
            .Received(1)
            .HandleAsync(
                Arg.Is<AddExpenseCommand>(
                    c => c.MonthlyBillingId == monthlyBillingId
                      && c.PlanId == planId
                      && c.Money == 125.04M
                      && c.Currency == "PLN"
                      && c.ExpenseDate == new DateOnly(2023, 5, 1)
                      && c.Description == "TEST"
                ),
                Arg.Any<CancellationToken>()
            );
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
                new DateOnly(),
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
                new DateOnly(),
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
                new DateOnly(),
                "Updated description"
            )
        );

        // Assert
        await _mockUpdateExpenseCommandHandler
            .Received(1)
            .HandleAsync(
                Arg.Any<UpdateExpenseCommand>(),
                Arg.Any<CancellationToken>()
            );
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
        await _mockRemoveExpenseCommandHandler
            .Received(1)
            .HandleAsync(
                Arg.Any<RemoveExpenseCommand>(),
                Arg.Any<CancellationToken>()
            );
    }
}