using Application.MonthlyBillings.DTO;
using Domain.MonthlyBillings;

namespace Application.MonthlyBillings.Mappings;

public static class MonthlyBillingMappingsExtension
{
    public static MonthlyBillingDTO ToDTO(this MonthlyBilling domain)
    {
        if (domain is null)
        {
            return null;
        }

        return new MonthlyBillingDTO()
        {
            Id = domain.Id.Value.ToString(),
            Year = domain.Year.Value,
            Month = domain.Month.Value,
            State = (int)domain.State,
            Incomes = domain.Incomes
                .Select(i => i.ToDTO())
                .ToList(),
            Plans = domain.Plans
                .Select(p => p.ToDTO())
                .ToList()
        };
    }
}