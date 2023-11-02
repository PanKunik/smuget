namespace Application.MonthlyBillings;

public sealed class IncomeDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Money { get; set; } = null!;
    public bool Include { get; set; }
}