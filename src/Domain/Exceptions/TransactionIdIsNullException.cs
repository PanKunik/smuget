using Domain.PiggyBanks;

namespace Domain.Exceptions;

public sealed class TransactionIdIsNullException
    : RequiredFieldException
{
    public TransactionIdIsNullException()
        : base(nameof(Transaction.Id)) { }
}
