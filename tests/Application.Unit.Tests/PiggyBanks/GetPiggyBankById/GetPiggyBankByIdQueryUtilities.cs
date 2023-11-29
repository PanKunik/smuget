using Application.PiggyBanks.GetPiggyBankById;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.PiggyBanks.GetPiggyBankById;

public static class GetPiggyBankByIdQueryUtilities
{
    public static GetPiggyBankByIdQuery CreateQuery()
        => new(
            Constants.PiggyBank.Id,
            Constants.User.Id
        );
}