using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.CQRS;
using Application.MonthlyBillings.AddIncome;
using Application.MonthlyBillings.RemoveIncome;
using Application.MonthlyBillings.UpdateIncome;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WebAPI.Incomes;
using WebAPI.Services.Users;

namespace WebAPI.Unit.Tests.Incomes;

public sealed class IncomesControllerTests
{
    private readonly IncomesController _cut;

    private readonly ICommandHandler<AddIncomeCommand> _mockAddIncomeCommandHandler;
    private readonly ICommandHandler<UpdateIncomeCommand> _mockUpdateIncomeCommandHandler;
    private readonly ICommandHandler<RemoveIncomeCommand> _mockRemoveIncomeCommandHandler;
    private readonly IUserService _mockUserService;

    public IncomesControllerTests()
    {
        _mockAddIncomeCommandHandler = Substitute.For<ICommandHandler<AddIncomeCommand>>();
        _mockUpdateIncomeCommandHandler = Substitute.For<ICommandHandler<UpdateIncomeCommand>>();
        _mockRemoveIncomeCommandHandler = Substitute.For<ICommandHandler<RemoveIncomeCommand>>();
        _mockUserService = Substitute.For<IUserService>();

        _mockUserService
            .UserId
            .Returns(Guid.NewGuid());

        _cut = new IncomesController(
            _mockAddIncomeCommandHandler,
            _mockUpdateIncomeCommandHandler,
            _mockRemoveIncomeCommandHandler,
            _mockUserService
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
        await _mockAddIncomeCommandHandler
            .Received(1)
            .HandleAsync(
                Arg.Any<AddIncomeCommand>(),
                Arg.Any<CancellationToken>()
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
            include
        );

        // Act
        await _cut.AddIncome(
            monthlyBillingId,
            request,
            default
        );

        // Assert
        await _mockAddIncomeCommandHandler
            .Received(1)
            .HandleAsync(
                Arg.Is<AddIncomeCommand>(
                    a => a.MonthlyBillingId == monthlyBillingId
                      && a.Name == name
                      && a.Amount == amount
                      && a.Currency == currency
                      && a.Include == include
                ),
                Arg.Any<CancellationToken>()
            );
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
        await _mockUpdateIncomeCommandHandler
            .Received(1)
            .HandleAsync(
                Arg.Any<UpdateIncomeCommand>(),
                Arg.Any<CancellationToken>()
            );
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
        await _mockRemoveIncomeCommandHandler
            .Received(1)
            .HandleAsync(
                Arg.Any<RemoveIncomeCommand>(),
                Arg.Any<CancellationToken>()
            );
    }
}