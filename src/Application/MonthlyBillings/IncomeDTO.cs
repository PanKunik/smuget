namespace Application.MonthlyBillings;

public sealed class IncomeDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Money { get; set; }
    public bool Include { get; set; }
    public bool Active { get; set; }
}