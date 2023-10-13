using Application.MonthlyBillings.RemovePlan;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.TestUtilities;

public static class RemovePlanCommandUtilities
{
    public static RemovePlanCommand CreateCommand()
        => new(
            Constants.MonthlyBilling.Id,
            Constants.Plan.Id,
            Constants.User.Id
        );
}