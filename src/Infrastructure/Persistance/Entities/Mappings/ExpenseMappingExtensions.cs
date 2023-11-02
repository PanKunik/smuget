using Domain.MonthlyBillings;

namespace Infrastructure.Persistance.Entities.Mappings;

internal static class ExpenseMappingExtensions
{
    public static Expense ToDomain(this ExpenseEntity entity)
    {
        if (entity is null)
        {
            return null;
        }

        return new Expense(
            new ExpenseId(entity.Id),
            new Money(
                entity.MoneyAmount,
                new Currency(entity.MoneyCurrency)
            ),
            entity.ExpenseDate,
            entity.Description
        );
    }

    public static ExpenseEntity ToEntity(
        this Expense domain,
        Guid planId
    )
    {
        return new ExpenseEntity()
        {
            Id = domain.Id.Value,
            MoneyAmount = domain.Money.Amount,
            MoneyCurrency = domain.Money.Currency.Value,
            ExpenseDate = domain.ExpenseDate,
            Description = domain.Description,
            PlanId = planId,
            Active = domain.Active
        };
    }
}