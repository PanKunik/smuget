using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.CQRS;
using Application.MonthlyBillings.AddPlan;
using Application.MonthlyBillings.RemovePlan;
using Application.MonthlyBillings.UpdatePlan;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Plans;

namespace WebAPI.Unit.Tests.Plans;

public sealed class PlansControllerTests
{
    private readonly PlansController _cut;

    private readonly Mock<ICommandHandler<AddPlanCommand>> _mockAddPlanCommandHandler;
    private readonly Mock<ICommandHandler<RemovePlanCommand>> _mockRemovePlanCommandHandler;
    private readonly Mock<ICommandHandler<UpdatePlanCommand>> _mockUpdatePlanCommandHandler;

    public PlansControllerTests()
    {
        _mockAddPlanCommandHandler = new Mock<ICommandHandler<AddPlanCommand>>();
        _mockRemovePlanCommandHandler = new Mock<ICommandHandler<RemovePlanCommand>>();
        _mockUpdatePlanCommandHandler = new Mock<ICommandHandler<UpdatePlanCommand>>();

        _cut = new PlansController(
            _mockAddPlanCommandHandler.Object,
            _mockUpdatePlanCommandHandler.Object,
            _mockRemovePlanCommandHandler.Object
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
        _mockAddPlanCommandHandler.Verify(m => m.HandleAsync(
            It.IsAny<AddPlanCommand>(),
            default), Times.Once
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
        _mockUpdatePlanCommandHandler.Verify(
            m => m.HandleAsync(
                It.IsAny<UpdatePlanCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
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
        _mockRemovePlanCommandHandler.Verify(
            m => m.HandleAsync(
                It.IsAny<RemovePlanCommand>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

}
