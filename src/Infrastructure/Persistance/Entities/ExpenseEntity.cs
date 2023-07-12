namespace Infrastructure.Persistance.Entities;

internal sealed class ExpenseEntity
{
    public Guid Id { get; set; }
    public decimal MoneyAmount { get; set; }
    public string MoneyCurrency { get; set; }
    public DateTimeOffset ExpenseDate { get; set; }
    public string Description { get; set; }
    public Guid PlanId { get; set; }
}