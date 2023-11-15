using Domain.PiggyBanks;

namespace Domain.Exceptions;

public sealed class InvalidPiggyBankIdException
    : RequiredFieldException
{
    public InvalidPiggyBankIdException()
        : base(nameof(PiggyBankId)) { }
}