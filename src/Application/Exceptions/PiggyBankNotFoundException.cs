using Domain.Exceptions;
using Domain.PiggyBanks;

namespace Application.Exceptions;

public sealed class PiggyBankNotFoundException
    : NotFoundException
{
    public PiggyBankNotFoundException(PiggyBankId piggyBankId)
        : base(
            nameof(PiggyBank),
            nameof(PiggyBank.Id),
            piggyBankId.Value.ToString()
        ) { }
}
