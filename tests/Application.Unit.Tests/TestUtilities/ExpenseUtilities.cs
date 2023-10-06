using Domain.MonthlyBillings;

namespace Application.Unit.Tests.TestUtilities;

public static class ExpenseUtilities
{
    public static Expense CreateExpense()
        => new(
            new(Constants.Constants.Expense.Id),
            new(
                Constants.Constants.Expense.Money,
                new(Constants.Constants.Expense.Currency)
            ),
            Constants.Constants.Expense.ExpenseDate,
            new(Constants.Constants.Expense.Description)
        );
}