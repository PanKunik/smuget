using Application.MonthlyBillings.UpdateExpense;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.TestUtilities;

public static class UpdateExpenseCommandUtilities
{
    public static UpdateExpenseCommand CreateCommand()
        => new(
            Constants.MonthlyBilling.Id,
            Constants.Plan.Id,
            Constants.Expense.Id,
            456.42m,
            "USD",
            new DateOnly(2023, 9, 11),
            "Updated description",
            Constants.User.Id
        );
}