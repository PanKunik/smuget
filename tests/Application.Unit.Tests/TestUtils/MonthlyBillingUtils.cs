using Domain.MonthlyBillings;

namespace Application.Unit.Tests.TestUtils;

public static class MonthlyBillingUtils
{
    public static MonthlyBilling CreateMonthlyBilling(
        List<Plan> plans = null,
        List<Income> incomes = null
    )
        => new(
            new(Constants.Constants.MonthlyBilling.Id),
            new(Constants.Constants.MonthlyBilling.Year),
            new(Constants.Constants.MonthlyBilling.Month),
            new(Constants.Constants.MonthlyBilling.Currency),
            Constants.Constants.MonthlyBilling.State,
            plans,
            incomes
        );
}