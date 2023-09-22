using Application.MonthlyBillings.Commands.OpenMonthlyBilling;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.Commands.TestUtilities;

public static class OpenMonthlyBilingCommandUtilities
{
    public static OpenMonthlyBillingCommand CreateCommand()
        => new (
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency
        );
}