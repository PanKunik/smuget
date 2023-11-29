namespace Application.PiggyBanks;

public sealed class TransactionDTO
{
    public Guid Id { get; set; }
    public decimal Value { get; set; }
    public DateOnly Date { get; set; }
}