using Application.MonthlyBillings.DTO;
using Domain.MonthlyBillings;

namespace Application.MonthlyBillings.Mappings;

public static class IncomeMappings
{
    public static IncomeDTO ToDTO(this Income domain)
    {
        if (domain is null)
        {
            return null;
        }

        return new IncomeDTO()
        {
            Id = domain.Id.Value,
            Name = domain.Name.Value,
            Money = domain.Money.ToString(),
            Include = domain.Include
        };
    }
}