using Domain.PiggyBanks;

namespace Domain.Unit.Tests.PiggyBanks.TestUtilities;

public static class PiggyBankUtilities
{
    public static PiggyBank CreatePiggyBank()
        => new(
            Constants.PiggyBank.Id,
            Constants.PiggyBank.Name,
            Constants.PiggyBank.WithGoal,
            Constants.PiggyBank.Goal,
            Constants.PiggyBank.UserId
        );
}