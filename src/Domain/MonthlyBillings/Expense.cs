using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed class Expense
{
    public ExpenseId Id { get; } = new ExpenseId(Guid.NewGuid());
    public Money Money { get; }
    public DateTimeOffset ExpenseDate { get; }
    public string Description { get; }
    public bool Active { get; private set; } = true;

    public Expense(
        ExpenseId expenseId,
        Money money,
        DateTimeOffset expenseDate,
        string description
    )
    {
        if (expenseId is null)
        {
            throw new ExpenseIdIsNullException();
        }

        Id = expenseId;

        if (money is null)
        {
            throw new MoneyIsNullException();
        }

        Money = money;
        ExpenseDate = expenseDate;
        Description = description;
    }

    internal void Remove()
    {
        Active = false;
    }
}