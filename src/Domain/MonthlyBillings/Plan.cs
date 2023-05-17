using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed class Plan
{
    public PlanId Id { get; private set; } = new PlanId(Guid.NewGuid());
    public Category Category { get; private set; }
    public Money MoneyAmount { get; private set; }
    public uint SortOrder { get; private set; }

    public Plan(
        Category category,
        Money amount,
        uint sortOrder
    )
    {
        if (category is null)
        {
            throw new CategoryIsNullException();
        }

        Category = category;

        if (amount is null)
        {
            throw new MoneyIsNullException();
        }

        MoneyAmount = amount;
        SortOrder = sortOrder;
    }

    private Plan() { }
}