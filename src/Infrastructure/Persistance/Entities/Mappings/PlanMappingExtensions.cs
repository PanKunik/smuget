using Domain.MonthlyBillings;

namespace Infrastructure.Persistance.Entities.Mappings;

internal static class PlanMappingExtensions
{
    public static Plan ToDomain(this PlanEntity entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new Plan(
            new PlanId(entity.Id),
            new Category(entity.Category),
            new Money(
                entity.MoneyAmount,
                new Currency(entity.MoneyCurrency)
            ),
            entity.SortOrder,
            entity.Expenses.ConvertAll(
                e => e.ToDomain()
            )
        );
    }

    public static PlanEntity ToEntity(this Plan domain, Guid monthlyBillingId)
    {
        return new PlanEntity()
        {
            Id = domain.Id.Value,
            Category = domain.Category.Value,
            MoneyAmount = domain.Money.Amount,
            MoneyCurrency = domain.Money.Currency.Value,
            SortOrder = domain.SortOrder,
            MonthlyBillingId = monthlyBillingId,
            Expenses = domain.Expenses.Select(
                e => e.ToEntity(domain.Id.Value)
            )
            .ToList()
        };
    }
}