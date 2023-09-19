using Application.MonthlyBillings.Queries.GetByYearAndMonth;
using Application.Unit.Tests.TestUtils.Constants;

namespace Application.Unit.Tests.MonthlyBillings.Queries.TestUtils;

public static class GetMonthlyBillingByYearAndMonthQueryUtils
{
    public static GetMonthlyBillingByYearAndMonthQuery CreateQuery()
        => new(
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month
        );
}