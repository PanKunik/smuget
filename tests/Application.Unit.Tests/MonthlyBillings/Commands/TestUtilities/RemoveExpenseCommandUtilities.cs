using Application.MonthlyBillings.Commands.RemoveExpense;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.Commands.TestUtilities;

public static class RemoveExpenseCommandUtilities
{
    public static RemoveExpenseCommand CreateCommand()
        => new(
            Constants.MonthlyBilling.Id,
            Constants.Plan.Id,
            Constants.Expense.Id
        );
}