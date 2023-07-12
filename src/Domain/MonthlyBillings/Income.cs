using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed class Income // TODO: Make internal?
{
    public IncomeId Id { get; } = new IncomeId(Guid.NewGuid()); // TODO: Remove identity from this class
    public Name Name { get; }
    public Money Money { get; }
    public bool Include { get; }

    public Income(
        IncomeId incomeId,
        Name name,
        Money money,
        bool include
    )
    {
        if (incomeId is null)
        {
            throw new IncomeIdIsNullException();
        }

        Id = incomeId;

        if (name is null)
        {
            throw new NameIsNullException();
        }

        Name = name;

        if (money is null)
        {
            throw new MoneyIsNullException();
        }

        Money = money;
        Include = include;
    }

    private Income() { }
}