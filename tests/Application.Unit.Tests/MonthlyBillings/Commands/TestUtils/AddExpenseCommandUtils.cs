using Application.MonthlyBillings.Commands.AddExpense;
using Application.Unit.Tests.TestUtils.Constants;

namespace Application.Unit.Tests.MonthlyBillings.Commands.TestUtils;

public static class AddExpenseCommandUtils
{
    public static AddExpenseCommand CreateCommand()
        => new(
            Constants.MonthlyBilling.Id,
            Constants.Plan.Id,
            Constants.Expense.Money,
            Constants.Expense.Currency,
            Constants.Expense.ExpenseDate,
            Constants.Expense.Description
        );
}