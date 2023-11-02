using Domain.MonthlyBillings;

namespace Application.MonthlyBillings.Mappings;

public static class MonthlyBillingMappingsExtension
{
    public static MonthlyBillingDTO? ToDTO(this MonthlyBilling domain)
    {
        if (domain is null)
        {
            return null; //TODO: Throw?
        }

        return new MonthlyBillingDTO()
        {
            Id = domain.Id.Value,
            Year = domain.Year.Value,
            Month = domain.Month.Value,
            State = domain.State.ToString(),
            Incomes = domain.Incomes?
                .Select(i => i.ToDTO())
                .ToList() ?? new List<IncomeDTO?>(),
            Plans = domain.Plans?
                .Select(p => p.ToDTO())
                .ToList() ?? new List<PlanDTO?>(),
            SumOfIncome = domain.SumOfIncome,
            SumOfIncomeAvailableForPlanning = domain.SumOfIncomeAvailableForPlanning,
            SumOfPlan = domain.SumOfPlan,
            SumOfExpenses = domain.SumOfExpenses,
            AccountBalance = domain.AccountBalance,
            SavingsForecast = domain.SavingsForecast
        };
    }
}