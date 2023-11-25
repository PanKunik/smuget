using Domain.PiggyBanks;

namespace Domain.Exceptions;

public sealed class TransactionValueEqualToZeroException
    : ValidationException
{
    public TransactionValueEqualToZeroException()
        : base(
            $"Value fo the transaction cannot be equal to 0.",
            nameof(Transaction.Value)
        ) { }
}