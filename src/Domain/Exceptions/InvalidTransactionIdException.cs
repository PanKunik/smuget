using Domain.PiggyBanks;

namespace Domain.Exceptions;

public sealed class InvalidTransactionIdException
    : RequiredFieldException
{
    public InvalidTransactionIdException()
        : base(nameof(TransactionId)) { }
}
