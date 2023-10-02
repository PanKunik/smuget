using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed class Expense
{
    public ExpenseId Id { get; } = new ExpenseId(Guid.NewGuid());
    public Money Money { get; private set; }
    public DateTimeOffset ExpenseDate { get; private set; }
    public string Description { get; private set; }
    public bool Active { get; private set; } = true;

    public Expense(
        ExpenseId expenseId,
        Money money,
        DateTimeOffset expenseDate,
        string description
    )
    {
        ThrowIfExpenseIdIsNull(expenseId);
        ThrowIfMoneyIsNull(money);

        Id = expenseId;
        Money = money;
        ExpenseDate = expenseDate;
        Description = description;
    }

    private void ThrowIfExpenseIdIsNull(ExpenseId expenseId)
    {
        if (expenseId is null)
        {
            throw new ExpenseIdIsNullException();
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
        Money money,
        DateTimeOffset expenseDate,
        string description
    )
    {
        ThrowIfMoneyIsNull(money);

        Money = money;
        ExpenseDate = expenseDate;
        Description = description;
    }

    internal void Remove()
    {
        Active = false;
    }
}