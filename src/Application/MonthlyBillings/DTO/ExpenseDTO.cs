namespace Application.MonthlyBillings.DTO;

public sealed class ExpenseDTO
{
    public string Id { get; set; }
    public string Money { get; set; }
    public DateTimeOffset ExpenseDate { get; set; }
    public string Description { get; set; }
}