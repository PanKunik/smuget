using Domain.MonthlyBillings;

namespace Application.Unit.Tests.TestUtilities;

public static class MonthlyBillingUtilities
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
            plans ?? CreatePlans(),
            incomes ?? CreateIncomes()
        );

    public static List<Plan> CreatePlans(
        List<Expense> expenses = null,
        int plansCount = 1
    )
        => Enumerable
            .Range(0, plansCount)
            .Select(r => new Plan(
                new PlanId(Guid.NewGuid()),
                Constants.Constants.Plan.CategoryFromIndex(r),
                Constants.Constants.Plan.MoneyFromIndex(r),
                (uint)(r + 1),
                expenses ?? CreateExpenses()
            ))
            .ToList();

    public static List<Income> CreateIncomes(int incomesCount = 1)
        => Enumerable
            .Range(0, incomesCount)
            .Select(r => new Income(
                new IncomeId(Guid.NewGuid()),
                Constants.Constants.Income.NameFromIndex(r),
                Constants.Constants.Income.MoneyFromIndex(r),
                Constants.Constants.Income.IncludeFromIndex(r)
            ))
            .ToList();


    public static List<Expense> CreateExpenses(int expensesCount = 2)
        => Enumerable
            .Range(0, expensesCount)
            .Select(r => new Expense(
                new ExpenseId(Guid.NewGuid()),
                Constants.Constants.Expense.MoneyFromIndex(r),
                Constants.Constants.Expense.ExpenseDateFromIndex(r),
                Constants.Constants.Expense.DescriptionFromIndex(r)
            ))
            .ToList();
}