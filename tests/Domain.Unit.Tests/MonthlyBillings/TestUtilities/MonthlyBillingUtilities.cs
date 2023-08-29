using System;
using System.Collections.Generic;
using System.Linq;
using Domain.MonthlyBillings;

namespace Domain.Unit.Tests.MonthlyBillings.TestUtilities;

public static class MonthlyBillingUtilities
{
    public static MonthlyBilling CreateMonthlyBilling(
        List<Plan> plans = null,
        List<Income> incomes = null
    )
        => new(
            Constants.MonthlyBilling.Id,
            Constants.MonthlyBilling.Year,
            Constants.MonthlyBilling.Month,
            Constants.MonthlyBilling.Currency,
            Constants.MonthlyBilling.State,
            plans ?? CreatePlans(),
            incomes ?? CreateIncomes()
        );

    public static List<Plan> CreatePlans(int plansCount = 1)
        => Enumerable
            .Range(0, plansCount)
            .Select(r => new Plan(
                new PlanId(Guid.NewGuid()),
                Constants.Plan.CategoryFromIndex(r),
                Constants.Plan.MoneyFromIndex(r),
                (uint)(r + 1)
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
}