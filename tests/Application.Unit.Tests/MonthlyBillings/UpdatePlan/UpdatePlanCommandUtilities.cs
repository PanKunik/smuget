using Application.MonthlyBillings.UpdatePlan;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.TestUtilities;

public static class UpdatePlanCommandUtilities
{
    public static UpdatePlanCommand CreateCommand()
        => new(
            Constants.MonthlyBilling.Id,
            Constants.Plan.Id,
            "Updated Category",
            258.96m,
            "EUR",
            99,
            Constants.User.Id
        );
}