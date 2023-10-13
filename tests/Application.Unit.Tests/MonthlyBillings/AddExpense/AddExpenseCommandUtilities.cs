using Application.MonthlyBillings.AddExpense;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.TestUtilities;

public static class AddExpenseCommandUtilities
{
    public static AddExpenseCommand CreateCommand()
        => new(
            Constants.MonthlyBilling.Id,
            Constants.Plan.Id,
            Constants.Expense.Money,
            Constants.Expense.Currency,
            Constants.Expense.ExpenseDate,
            Constants.Expense.Description,
            Constants.User.Id
        );
}