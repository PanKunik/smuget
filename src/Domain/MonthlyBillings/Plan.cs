using System.Runtime.CompilerServices;
using Domain.Exceptions;

[assembly: InternalsVisibleTo("Domain.Unit.Tests")]
namespace Domain.MonthlyBillings;

public sealed class Plan
{
    public PlanId Id { get; } = new PlanId(Guid.NewGuid());
    public Category Category { get; private set; }
    public Money Money { get; private set; }
    public uint SortOrder { get; private set; }
    public bool Active { get; private set; } = true;
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
        // TODO: Refactor - extract method
        if (planId is null)
        {
            throw new PlanIdIsNullException();
        }

        Id = planId;

        // TODO: Refactor - extract method
        if (category is null)
        {
            throw new CategoryIsNullException();
        }

        Category = category;

        // TODO: Refactor - extract method
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

    internal void Update(
        Category category,
        Money money,
        uint sortOrder
    )
    {
        // TODO: Refactor - extract method
        if (category is null)
        {
            throw new CategoryIsNullException();
        }

        // TODO: Refactor - extract method
        if (money is null)
        {
            throw new MoneyIsNullException();
        }

        Category = category;
        Money = money;
        SortOrder = sortOrder;
    }

    internal void Remove()
    {
        Active = false;
    }
}