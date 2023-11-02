using Application.MonthlyBillings.OpenMonthlyBilling;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.TestUtilities;

public static class OpenMonthlyBilingCommandUtilities
{
    public static OpenMonthlyBillingCommand CreateCommand()
        => new(
            Constants.MonthlyBilling.Year + 1,
            Constants.MonthlyBilling.Month + 1,
            Constants.MonthlyBilling.Currency,
            Constants.User.Id
        );
}