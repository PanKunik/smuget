using Application.MonthlyBillings.GetByYearAndMonth;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.TestUtilities;

public static class GetMonthlyBillingByYearAndMonthQueryUtilities
{
    public static GetMonthlyBillingByYearAndMonthQuery CreateQuery()
        => new(
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month
        );
}