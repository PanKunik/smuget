using Domain.PiggyBanks;

namespace Domain.Exceptions;

public sealed class PiggyBankIdIsNullException
    : RequiredFieldException
{
    public PiggyBankIdIsNullException()
        : base(nameof(PiggyBankId)) { }
}