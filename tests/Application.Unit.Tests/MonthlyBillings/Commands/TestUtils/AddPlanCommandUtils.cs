using Application.MonthlyBillings.Commands.AddPlan;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.Commands.TestUtils;

public static class AddPlanCommandUtils
{
    public static AddPlanCommand CreateCommand()
        => new(
            Constants.MonthlyBilling.Id,
            Constants.Plan.Category,
            Constants.Plan.MoneyAmount,
            Constants.Plan.Currency,
            Constants.Plan.SortOrder
        );
}