using Application.MonthlyBillings.AddPlan;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.TestUtilities;

public static class AddPlanCommandUtilities
{
    public static AddPlanCommand CreateCommand()
        => new(
            Constants.MonthlyBilling.Id,
            Constants.Plan.Category,
            Constants.Plan.MoneyAmount,
            Constants.Plan.Currency,
            Constants.Plan.SortOrder,
            Constants.User.Id
        );
}