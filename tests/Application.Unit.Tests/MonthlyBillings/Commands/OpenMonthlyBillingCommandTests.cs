using System.Threading.Tasks;
using Application.MonthlyBillings.Commands;

namespace Application.Unit.Tests.MonthlyBillings.Commands;

public sealed class OpenMonthlyBillingCommandTests
{
    [Fact]
    public async Task HandleAsync_OnSuccess_ShouldPass()
    {
        // Arrange
        var command = new OpenMonthlyBillingCommand(2023, 4);
        var cut = new OpenMonthlyBillingCommandHandler();

        // Act
        await cut.HandleAsync(command, default);
    }
}