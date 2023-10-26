namespace Application.MonthlyBillings;

public sealed class ExpenseDTO
{
    public Guid Id { get; set; }
    public string Money { get; set; }
    public DateOnly ExpenseDate { get; set; }
    public string Description { get; set; }
}