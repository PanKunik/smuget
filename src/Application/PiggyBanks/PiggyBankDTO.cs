namespace Application.PiggyBanks;

public sealed class PiggyBankDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool WithGoal { get; set; }
    public decimal Goal { get; set; }
    public IEnumerable<TransactionDTO> Transactions { get; set; }
}