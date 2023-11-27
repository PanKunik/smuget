using Domain.PiggyBanks;

namespace Domain.Exceptions;

public sealed class TransactionAlreadyExistsException
    : ConflictException
{
    public TransactionAlreadyExistsException(TransactionId id)
        : base(
            nameof(Transaction),
            nameof(Transaction.Id),
            id.Value.ToString()
        ) { }
}