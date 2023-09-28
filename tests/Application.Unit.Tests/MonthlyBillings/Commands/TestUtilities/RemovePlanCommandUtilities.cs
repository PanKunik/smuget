using Application.MonthlyBillings.Commands.RemovePlan;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.Commands.TestUtilities;

public static class RemovePlanCommandUtilities
{
    public static RemovePlanCommand CreateCommand()
        => new(
            Guid.NewGuid(),
            Constants.Plan.Id
        );
}