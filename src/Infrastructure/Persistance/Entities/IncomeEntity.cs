namespace Infrastructure.Persistance.Entities;

internal sealed class IncomeEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal MoneyAmount { get; set; }
    public string MoneyCurrency { get; set; }
    public bool Include { get; set; }
    public Guid MonthlyBillingId { get; set; }
}