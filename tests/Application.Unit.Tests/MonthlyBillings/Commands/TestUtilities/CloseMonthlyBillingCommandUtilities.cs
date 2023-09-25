using Application.MonthlyBillings.Commands.CloseMonthlyBilling;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.Commands.TestUtilities;

public static class CloseMonthlyBillingCommandUtilities
{
    public static CloseMonthlyBillingCommand CreateCommand()
        => new(
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month
        );
}