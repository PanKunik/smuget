using Domain.MonthlyBillings;

namespace Application.Unit.Tests.TestUtilities;

public static class PlansUtilities
{
    public static Plan CreatePlan(List<Expense>? expenses = null)
        => new(
            new(Constants.Constants.Plan.Id),
            new(Constants.Constants.Plan.Category),
            new(
                Constants.Constants.Plan.MoneyAmount,
                new(Constants.Constants.Plan.Currency)
            ),
            1,
            expenses
        );
}