using Application.MonthlyBillings.Queries.GetByYearAndMonth;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.Queries.TestUtilities;

public static class GetMonthlyBillingByYearAndMonthQueryUtils
{
    public static GetMonthlyBillingByYearAndMonthQuery CreateQuery()
        => new(
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month
        );
}