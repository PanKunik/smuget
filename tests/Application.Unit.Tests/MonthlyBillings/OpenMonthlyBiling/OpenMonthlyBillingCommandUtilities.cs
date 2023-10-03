using Application.MonthlyBillings.OpenMonthlyBilling;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.TestUtilities;

public static class OpenMonthlyBilingCommandUtilities
{
    public static OpenMonthlyBillingCommand CreateCommand()
        => new (
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency
        );
}