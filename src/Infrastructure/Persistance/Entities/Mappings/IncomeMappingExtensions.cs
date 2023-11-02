using Domain.MonthlyBillings;

namespace Infrastructure.Persistance.Entities.Mappings;

internal static class IncomeMappingExtensions
{
    public static Income ToDomain(this IncomeEntity entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new Income(
            new IncomeId(entity.Id),
            new Name(entity.Name),
            new Money(
                entity.MoneyAmount,
                new Currency(entity.MoneyCurrency)
            ),
            entity.Include
        );
    }

    public static IncomeEntity ToEntity(
        this Income domain,
        Guid monthlyBillingId
    )
    {
        return new IncomeEntity()
        {
            Id = domain.Id.Value,
            Name = domain.Name.Value,
            MoneyAmount = domain.Money.Amount,
            MoneyCurrency = domain.Money.Currency.Value,
            Include = domain.Include,
            Active = domain.Active,
            MonthlyBillingId = monthlyBillingId
        };
    }
}