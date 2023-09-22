using Application.MonthlyBillings.DTO;
using Domain.MonthlyBillings;

namespace Application.MonthlyBillings.Mappings;

public static class PlanMappings
{
    public static PlanDTO ToDTO(this Plan domain)
    {
        if (domain is null)
        {
            return null;
        }

        return new PlanDTO()
        {
            Id = domain.Id.Value.ToString(),
            Category = domain.Category.Value,
            Money = domain.Money.ToString(),
            SortOrder = domain.SortOrder,
            Expenses = domain.Expenses
                .Select(e => e.ToDTO())
                .ToList(),
            SumOfExpenses = domain.SumOfExpenses
        };
    }
}