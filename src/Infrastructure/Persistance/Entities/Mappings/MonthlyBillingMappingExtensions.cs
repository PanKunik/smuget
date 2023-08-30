using Domain.MonthlyBillings;

namespace Infrastructure.Persistance.Entities.Mappings;

internal static class MonthlyBillingMappingExtensions
{
    public static MonthlyBilling ToDomain(this MonthlyBillingEntity entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new MonthlyBilling(
            new MonthlyBillingId(entity.Id),
            new Year(entity.Year),
            new Month(entity.Month),
            new Currency(entity.Currency),
            State.GetByName(entity.State),
            entity.Plans.ConvertAll(
                p => p.ToDomain()
            ),
            entity.Incomes.ConvertAll(
                i => i.ToDomain()
            )
        );
    }

    public static MonthlyBillingEntity ToEntity(this MonthlyBilling domain)
    {
        return new MonthlyBillingEntity()
        {
            Id = domain.Id.Value,
            Year = domain.Year.Value,
            Month = domain.Month.Value,
            State = domain.State.ToString(),
            Currency = domain.Currency.Value,
            Plans = domain.Plans.Select(
                p => p.ToEntity(domain.Id.Value)
            )
            .ToList(),
            Incomes = domain.Incomes.Select(
                i => i.ToEntity(domain.Id.Value)
            )
            .ToList()
        };
    }
}