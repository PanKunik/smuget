using Domain.Exceptions;

namespace Domain.PiggyBanks;

public sealed record TransactionId
{
    public Guid Value { get; }

    public TransactionId(Guid value)
    {
        ThrowIfValueIsEqualToEmptyGuid(value);
        Value = value;
    }

    private void ThrowIfValueIsEqualToEmptyGuid(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidTransactionIdException();
        }
    }
}