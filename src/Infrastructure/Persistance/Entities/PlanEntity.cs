namespace Infrastructure.Persistance.Entities;

internal sealed class PlanEntity
{
    public Guid Id { get; set; }
    public string Category { get; set; }
    public decimal MoneyAmount { get; set; }
    public string MoneyCurrency { get; set; }
    public uint SortOrder { get; set; }
    public Guid MonthlyBillingId { get; set; }
    public List<ExpenseEntity> Expenses { get; set; }
}