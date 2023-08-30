namespace Application.MonthlyBillings.DTO;

public sealed class MonthlyBillingDTO
{
    public string Id { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public string State { get; set; }
    public IEnumerable<IncomeDTO> Incomes { get; set; }
    public IEnumerable<PlanDTO> Plans { get; set; }
}