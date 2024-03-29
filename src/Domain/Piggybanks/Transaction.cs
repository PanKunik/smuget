using Domain.Exceptions;

namespace Domain.PiggyBanks;

public sealed class Transaction
{
    public TransactionId Id { get; }
    public decimal Value { private set; get; }
    public DateOnly Date { private set; get; }
    public bool Active { get; set; } = true;

    public Transaction(
        TransactionId id,
        decimal value,
        DateOnly date
    )
    {
        ThrowIfTransactionIdIsNull(id);
        ThrowIfValueIsEqualToZero(value);

        Id = id;
        Value = value;
        Date = date;
    }

    private void ThrowIfTransactionIdIsNull(TransactionId id)
    {
        if (id is null)
        {
            throw new TransactionIdIsNullException();
        }
    }

    private void ThrowIfValueIsEqualToZero(decimal value)
    {
        if (value is 0.0m)
        {
            throw new TransactionValueEqualToZeroException();
        }
    }

    public void Remove()
    {
        Active = false;
    }

    public void Update(
        decimal value,
        DateOnly date
    )
    {
        ThrowIfValueIsEqualToZero(value);

        Value = value;
        Date = date;
    }
}