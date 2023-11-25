using Domain.Exceptions;
using Domain.MonthlyBillings;
using Domain.PiggyBanks;

namespace Application.Exceptions;

public sealed class PiggyBankAlreadyExistsException
    : ConflictException
{
    public PiggyBankAlreadyExistsException(Name name)
        : base(
            nameof(PiggyBank),
            nameof(PiggyBank.Name),
            name.Value
        ) { }
}
