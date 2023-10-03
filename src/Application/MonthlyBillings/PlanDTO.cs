namespace Application.MonthlyBillings;

public sealed class PlanDTO
{
    public string Id { get; set; }
    public string Category { get; set; }
    public string Money { get; set; }
    public uint SortOrder { get; set; }
    public IEnumerable<ExpenseDTO> Expenses { get; set; }
    public decimal SumOfExpenses { get; set; }
}