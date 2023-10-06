using Domain.MonthlyBillings;

namespace Application.Unit.Tests.TestUtilities;

public static class MonthlyBillingUtilities
{
    public static MonthlyBilling CreateMonthlyBilling(
        List<Plan>? plans = null,
        List<Income>? incomes = null
    )
        => new(
            new(Constants.Constants.MonthlyBilling.Id),
            new(Constants.Constants.MonthlyBilling.Year),
            new(Constants.Constants.MonthlyBilling.Month),
            new(Constants.Constants.MonthlyBilling.Currency),
            Constants.Constants.MonthlyBilling.State,
            plans ?? CreatePlans(),
            incomes ?? CreateIncomes()
        );

    public static List<Plan> CreatePlans(
        List<Expense>? expenses = null,
        byte plansCount = 1
    )
        => Enumerable
            .Range(0, plansCount)
            .Select(r => new Plan(
                Constants.Constants.Plan.IdFromIndex((byte)r),
                Constants.Constants.Plan.CategoryFromIndex((byte)r),
                Constants.Constants.Plan.MoneyFromIndex((byte)r),
                Constants.Constants.Plan.SortOrderFromIndex((byte)r),
                expenses ?? CreateExpenses()
            ))
            .ToList();

    public static List<Income> CreateIncomes(byte incomesCount = 1)
        => Enumerable
            .Range(0, incomesCount)
            .Select(r => new Income(
                Constants.Constants.Income.IdFromIndex((byte)r),
                Constants.Constants.Income.NameFromIndex((byte)r),
                Constants.Constants.Income.MoneyFromIndex((byte)r),
                Constants.Constants.Income.IncludeFromIndex((byte)r)
            ))
            .ToList();


    public static List<Expense> CreateExpenses(byte expensesCount = 2)
        => Enumerable
            .Range(0, expensesCount)
            .Select(r => new Expense(
                Constants.Constants.Expense.IdFromIndex((byte)r),
                Constants.Constants.Expense.MoneyFromIndex((byte)r),
                Constants.Constants.Expense.ExpenseDateFromIndex((byte)r),
                Constants.Constants.Expense.DescriptionFromIndex((byte)r)
            ))
            .ToList();
}