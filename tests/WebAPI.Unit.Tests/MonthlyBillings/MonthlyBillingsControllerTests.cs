using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.CQRS;
using Application.MonthlyBillings.Commands;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.MonthlyBillings;

namespace WebAPI.Unit.Tests.MonthlyBillings;

public sealed class MonthlyBillingsControllerTests
{
    private readonly Mock<ICommandHandler<OpenMonthlyBillingCommand>> _mockOpenMonthlyBillingCommandHandler;

    public MonthlyBillingsControllerTests()
    {
        _mockOpenMonthlyBillingCommandHandler = new Mock<ICommandHandler<OpenMonthlyBillingCommand>>();
    }

    [Fact]
    public async Task Open_OnSuccess_ShouldReturnCreatedObjectResult()
    {
        // Arrange
        var cut = new MonthlyBillingsController(_mockOpenMonthlyBillingCommandHandler.Object);

        // Act
        var result = await cut.Open(new(2020, 1));

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<CreatedResult>();
    }

    [Fact]
    public async Task Open_OnSuccess_ShouldReturnStatusCode201()
    {
        // Arrange
        var cut = new MonthlyBillingsController(_mockOpenMonthlyBillingCommandHandler.Object);

        // Act
        var result = (CreatedResult)await cut.Open(new(2020, 1));

        // Assert
        result.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task Open_WhenInvoked_ShouldCallOpenMonthlyBillingCommandHandler()
    {
        // Arrange
        var cut = new MonthlyBillingsController(_mockOpenMonthlyBillingCommandHandler.Object);

        // Act
        await cut.Open(new(2020, 1));

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
    public async Task Open_WhenInvoked_ShouldPassParametersToCommand(int year, int month)
    {
        // Arrange
        var cut = new MonthlyBillingsController(_mockOpenMonthlyBillingCommandHandler.Object);

        // Act
        await cut.Open(new(year, month));

        // Assert
        _mockOpenMonthlyBillingCommandHandler.Verify(
            m => m.HandleAsync(
                It.Is<OpenMonthlyBillingCommand>(
                    c => c.Year == year
                      && c.Month == month),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}