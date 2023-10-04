using Application.MonthlyBillings.RemovePlan;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.TestUtilities;

public static class RemovePlanCommandUtilities
{
    public static RemovePlanCommand CreateCommand()
        => new(
            Guid.NewGuid(),
            Constants.Plan.Id
        );
}