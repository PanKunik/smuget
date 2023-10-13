namespace Infrastructure.Persistance.Entities;

internal sealed class MonthlyBillingEntity
{
    public Guid Id { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public string State { get; set; }
    public string Currency { get; set; }
    public Guid UserId { get; set; }
    public List<IncomeEntity> Incomes { get; set; }
    public List<PlanEntity> Plans { get; set; }
}