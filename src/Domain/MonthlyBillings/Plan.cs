using System.Runtime.CompilerServices;
using Domain.Exceptions;

[assembly: InternalsVisibleTo("Domain.Unit.Tests")]
namespace Domain.MonthlyBillings;

public sealed class Plan
{
    public PlanId Id { get; } = new PlanId(Guid.NewGuid());
    public Category Category { get; }
    public Money Money { get; }
    public uint SortOrder { get; }
    public IReadOnlyCollection<Expense> Expenses => _expenses.AsReadOnly();    

    public decimal SumOfExpenses
        => _expenses?.Sum(e => e.Money.Amount) ?? 0m;

    private readonly List<Expense> _expenses = new();

    public Plan(
        PlanId planId,
        Category category,
        Money money,
        uint sortOrder,
        List<Expense> expenses = null
    )
    {
        if (planId is null)
        {
            throw new PlanIdIsNullException();
        }

        Id = planId;

        if (category is null)
        {
            throw new CategoryIsNullException();
        }

        Category = category;

        if (money is null)
        {
            throw new MoneyIsNullException();
        }

        Money = money;
        SortOrder = sortOrder;

        _expenses = expenses ?? new();
    }

    internal void AddExpense(Expense expense)
    {
        _expenses.Add(expense);
    }
}