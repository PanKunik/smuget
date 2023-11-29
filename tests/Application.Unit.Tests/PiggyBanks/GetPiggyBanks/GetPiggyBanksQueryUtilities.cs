using Application.PiggyBanks.GetPiggyBanks;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.PiggyBanks.GetPiggyBanks;

public static class GetPiggyBanksQueryUtilities
{
    public static GetPiggyBanksQuery CreateQuery()
        => new GetPiggyBanksQuery(
            Constants.User.Id
        );
}