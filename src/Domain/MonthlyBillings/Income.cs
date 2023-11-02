using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed class Income // TODO: Make internal?
{
    public IncomeId Id { get; } = new IncomeId(Guid.NewGuid());
    public Name Name { get; private set; }
    public Money Money { get; private set; }
    public bool Include { get; private set; }
    public bool Active { get; private set; } = true;

    public Income(
        IncomeId incomeId,
        Name name,
        Money money,
        bool include
    )
    {
        ThrowIfIncomeIdIsNull(incomeId);
        ThrowIfNameIsNull(name);
        ThrowIfMoneyIsNull(money);

        Id = incomeId;
        Name = name;
        Money = money;
        Include = include;
    }

    private void ThrowIfIncomeIdIsNull(IncomeId incomeId)
    {
        if (incomeId is null)
        {
            throw new IncomeIdIsNullException();
        }
    }

    private void ThrowIfNameIsNull(Name name)
    {
        if (name is null)
        {
            throw new NameIsNullException();
        }
    }

    private void ThrowIfMoneyIsNull(Money money)
    {
        if (money is null)
        {
            throw new MoneyIsNullException();
        }
    }

    internal void Update(
        Name name,
        Money money,
        bool include
    )
    {
        ThrowIfNameIsNull(name);
        ThrowIfMoneyIsNull(money);

        Name = name;
        Money = money;
        Include = include;
    }

    internal void Remove()
    {
        Active = false;
    }
}