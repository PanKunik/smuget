namespace Infrastructure.Persistance.Entities;

internal sealed class TransactionEntity
{
    public Guid Id { get; set; }
    public decimal Value { get; set; }
    public DateOnly Date { get; set; }
    public Guid PiggyBankId { get; set; }
}