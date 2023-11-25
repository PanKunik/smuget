namespace Infrastructure.Persistance.Entities;

internal sealed class PiggyBankEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool WithGoal { get; set; }
    public decimal Goal { get; set; }
    public List<TransactionEntity> Transactions { get; set; }
}