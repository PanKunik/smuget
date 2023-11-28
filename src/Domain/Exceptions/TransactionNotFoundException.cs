using Domain.PiggyBanks;

namespace Domain.Exceptions;

public sealed class TransactionNotFoundException
    : NotFoundException
{
    public TransactionNotFoundException(TransactionId transactionId)
        : base(
            nameof(Transaction),
            nameof(Transaction.Id),
            transactionId.Value.ToString()    
        ) { }
}
