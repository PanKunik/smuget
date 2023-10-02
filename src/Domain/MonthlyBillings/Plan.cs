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
        List<Expense>? expenses = null
    )
    {
        ThrowIfPlanIdIsNull(planId);
        ThrowIfCategoryIsNull(category);
        ThrowIfMoneyIsNull(money);

        Id = planId;
        Category = category;
        Money = money;
        SortOrder = sortOrder;

        _expenses = expenses ?? new();
    }

    private void ThrowIfPlanIdIsNull(PlanId planId)
    {
        if (planId is null)
        {
            throw new PlanIdIsNullException();
        }
    }

    private void ThrowIfCategoryIsNull(Category category)
    {
        if (category is null)
        {
            throw new CategoryIsNullException();
        }
    }

    private void ThrowIfMoneyIsNull(Money money)
    {
        if (money is null)
        {
            throw new MoneyIsNullException();
        }
    }

    internal void AddExpense(Expense expense)
    {
        _expenses.Add(expense);
    }

    internal void UpdateExpense(
        ExpenseId expenseId,
        Money money,
        DateTimeOffset expenseDate,
        string description
    )
    {
        ThrowIfExpenseIdIsNull(expenseId);

        var expenseToUpdate = _expenses?.Find(e => e.Id == expenseId && e.Active)
            ?? throw new ExpenseNotFoundException();

        expenseToUpdate.Update(
            money,
            expenseDate,
            description
        );
    }

    internal void RemoveExpense(ExpenseId expenseId)
    {
        ThrowIfExpenseIdIsNull(expenseId);

        var expenseToRemove = _expenses?.Find(e => e.Id == expenseId && e.Active)
            ?? throw new ExpenseNotFoundException();

        expenseToRemove.Remove();
    }

    private void ThrowIfExpenseIdIsNull(ExpenseId expenseId)
    {
        if (expenseId is null)
        {
            throw new ExpenseIdIsNullException();
        }
    }

    internal void Update(
        Category category,
        Money money,
        uint sortOrder
    )
    {
        ThrowIfCategoryIsNull(category);
        ThrowIfMoneyIsNull(money);

        Category = category;
        Money = money;
        SortOrder = sortOrder;
    }

    internal void Remove()
    {
        Active = false;
    }
}