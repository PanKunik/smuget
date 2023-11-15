using Domain.Exceptions;

namespace Domain.PiggyBanks;

public sealed record PiggyBankId
{
    public Guid Value { get; }

    public PiggyBankId(Guid value)
    {
        ThrowIfValueEqualsEmptyGuid(value);
        Value = value;
    }

    private static void ThrowIfValueEqualsEmptyGuid(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidPiggyBankIdException();
        }
    }
}