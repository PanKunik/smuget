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
            entity.State == 0 ? State.Open : State.Closed,  // TODO: Make VO
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
            State = (int)domain.State,
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