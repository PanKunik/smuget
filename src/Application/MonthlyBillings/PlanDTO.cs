namespace Application.MonthlyBillings;

public sealed class PlanDTO
{
    public Guid Id { get; set; }
    public string Category { get; set; } = null!;
    public string Money { get; set; } = null!;
    public uint SortOrder { get; set; }
    public IEnumerable<ExpenseDTO?> Expenses { get; set; } = null!;
    public decimal SumOfExpenses { get; set; }
}