using Domain.MonthlyBillings;

namespace Application.MonthlyBillings.Mappings;

public static class IncomeMappings
{
    public static IncomeDTO? ToDTO(this Income domain)
    {
        if (domain is null)
        {
            return null; //TODO: Throw?
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