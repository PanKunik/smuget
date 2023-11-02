using Domain.MonthlyBillings;

namespace Application.MonthlyBillings.Mappings;

public static class PlanMappings
{
    public static PlanDTO? ToDTO(this Plan domain)
    {
        if (domain is null)
        {
            return null; //TODO: Throw?
        }

        return new PlanDTO()
        {
            Id = domain.Id.Value,
            Category = domain.Category.Value,
            Money = domain.Money.ToString(),
            SortOrder = domain.SortOrder,
            Expenses = domain.Expenses?
                .Select(e => e.ToDTO())
                .ToList() ?? new List<ExpenseDTO?>(),
            SumOfExpenses = domain.SumOfExpenses
        };
    }
}