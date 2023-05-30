using System.Runtime.CompilerServices;
using Domain.Exceptions;

[assembly: InternalsVisibleTo("Domain.Unit.Tests")]
namespace Domain.MonthlyBillings;

public sealed class Plan
{
    public PlanId Id { get; private set; } = new PlanId(Guid.NewGuid());
    public Category Category { get; private set; }
    public Money MoneyAmount { get; private set; }
    public uint SortOrder { get; private set; }
    public IReadOnlyCollection<Expense> Expenses => _expenses.AsReadOnly();

    private readonly List<Expense> _expenses = new List<Expense>();

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

    internal void AddExpense(Expense expense)
    {
        _expenses.Add(expense);
    }

    private Plan() { }
}