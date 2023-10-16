using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.CQRS;
using Application.MonthlyBillings.AddPlan;
using Application.MonthlyBillings.RemovePlan;
using Application.MonthlyBillings.UpdatePlan;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using WebAPI.Plans;
using WebAPI.Services.Users;

namespace WebAPI.Unit.Tests.Plans;

public sealed class PlansControllerTests
{
    private readonly PlansController _cut;

    private readonly ICommandHandler<AddPlanCommand> _mockAddPlanCommandHandler;
    private readonly ICommandHandler<RemovePlanCommand> _mockRemovePlanCommandHandler;
    private readonly ICommandHandler<UpdatePlanCommand> _mockUpdatePlanCommandHandler;
    private readonly IUserService _mockUserService;

    public PlansControllerTests()
    {
        _mockAddPlanCommandHandler = Substitute.For<ICommandHandler<AddPlanCommand>>();
        _mockRemovePlanCommandHandler = Substitute.For<ICommandHandler<RemovePlanCommand>>();
        _mockUpdatePlanCommandHandler = Substitute.For<ICommandHandler<UpdatePlanCommand>>();
        _mockUserService = Substitute.For<IUserService>();

        _mockUserService
            .UserId
            .Returns(Guid.NewGuid());

        _cut = new PlansController(
            _mockAddPlanCommandHandler,
            _mockUpdatePlanCommandHandler,
            _mockRemovePlanCommandHandler,
            _mockUserService
        );
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
        result
            .Should()
            .NotBeNull();

        result
            .Should()
            .BeOfType<CreatedResult>();
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
        result.StatusCode
            .Should()
            .Be(201);
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
        await _mockAddPlanCommandHandler
            .Received(1)
            .HandleAsync(
                Arg.Any<AddPlanCommand>(),
                Arg.Any<CancellationToken>()
            );
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
        await _mockAddPlanCommandHandler
            .Received(1)
            .HandleAsync(
                Arg.Is<AddPlanCommand>(
                    c => c.Category == "Fuel"
                      && c.MoneyAmount == 87.96M
                      && c.Currency == "PLN"
                      && c.SortOrder == 5
                ),
                Arg.Any<CancellationToken>()
            );
    }

    [Fact]
    public async Task UpdatePlan_OnSuccess_ShouldReturnNoContentResult()
    {
        // Act
        var result = await _cut.UpdatePlan(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new(
                "Updated Category",
                11.24m,
                "USD",
                1
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
    public async Task UpdatePlan_OnSuccess_ShouldReturnStatusCode204()
    {
        // Act
        var result = (NoContentResult)await _cut.UpdatePlan(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new(
                "Updated Category",
                11.24m,
                "USD",
                12
            )
        );

        // Assert
        result.StatusCode
            .Should()
            .Be(204);
    }

    [Fact]
    public async Task UpdatePlan_WhenInvoked_ShouldCallUpdatePlanCommandHandler()
    {
        // Act
        await _cut.UpdatePlan(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new(
                "Updated name",
                11.24m,
                "USD",
                85
            )
        );

        // Assert
        await _mockUpdatePlanCommandHandler
            .Received(1)
            .HandleAsync(
                Arg.Any<UpdatePlanCommand>(),
                Arg.Any<CancellationToken>()
            );
    }

    [Fact]
    public async Task RemovePlan_OnSuccess_ShouldReturnNoContentResult()
    {
        // Act
        var result = await _cut.RemovePlan(
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
    public async Task RemovePlan_OnSuccess_ShouldReturn204StatusCode()
    {
        // Act
        var result = (NoContentResult)await _cut.RemovePlan(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Assert
        result.StatusCode
            .Should()
            .Be(204);
    }

    [Fact]
    public async Task RemovePlan_WhenInvoked_ShouldCallRemovePlanCommandHandler()
    {
        // Act
        await _cut.RemovePlan(
            Guid.NewGuid(),
            Guid.NewGuid()
        );

        // Assert
        await _mockRemovePlanCommandHandler
            .Received(1)
            .HandleAsync(
                Arg.Any<RemovePlanCommand>(),
                Arg.Any<CancellationToken>()
            );
    }

}
