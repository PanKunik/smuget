using System;
using System.Collections.Generic;
using System.Linq;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.TestUtilities;

public static class MonthlyBillingUtilities
{
    public static MonthlyBilling CreateMonthlyBilling(
        List<Plan> plans = null,
        List<Expense> expenses = null,
        List<Income> incomes = null
    )
        => new(
            Constants.MonthlyBilling.Id,
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency,
            Constants.MonthlyBilling.State,
            Constants.MonthlyBilling.UserId,
            plans ?? CreatePlans(expenses),
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
                Constants.Plan.CategoryFromIndex(r),
                Constants.Plan.MoneyFromIndex(r),
                (uint)(r + 1),
                expenses ?? CreateExpenses()
            ))
            .ToList();

    public static List<Income> CreateIncomes(int incomesCount = 1)
        => Enumerable
            .Range(0, incomesCount)
            .Select(r => new Income(
                new IncomeId(Guid.NewGuid()),
                Constants.Income.NameFromIndex(r),
                Constants.Income.MoneyFromIndex(r),
                true
            ))
            .ToList();

    public static List<Expense> CreateExpenses(int expensesCount = 2)
        => Enumerable
            .Range(0, expensesCount)
            .Select(r => new Expense(
                new ExpenseId(Guid.NewGuid()),
                Constants.Expense.MoneyFromIndex(r),
                Constants.Expense.ExpenseDateFromIndex(r),
                Constants.Expense.DescripitonFromIndex(r)
            ))
            .ToList();
}