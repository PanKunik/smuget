using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed class Expense
{
    public ExpenseId Id { get; } = new ExpenseId(Guid.NewGuid());
    public Money Money { get; }
    public DateTimeOffset ExpenseDate { get; }
    public string Descritpion { get; }

    public Expense(
        Money money,
        DateTimeOffset expenseDate,
        string description
    )
    {
        if (money is null)
        {
            throw new MoneyIsNullException();
        }

        Money = money;

        ExpenseDate = expenseDate;

        Descritpion = description;
    }

    #pragma warning disable CS8618
    private Expense() { }
    #pragma warning restore CS8618
}