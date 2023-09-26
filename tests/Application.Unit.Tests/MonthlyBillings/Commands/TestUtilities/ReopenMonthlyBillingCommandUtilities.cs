using Application.MonthlyBillings.Commands.ReopenMonthlyBilling;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.Commands.TestUtilities;

public static class ReopenMonthlyBillingCommandUtilities
{
    public static ReopenMonthlyBillingCommand CreateCommand()
        => new(
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month
        );
}