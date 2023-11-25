using Domain.PiggyBanks;

namespace Application.Unit.Tests.TestUtilities;

public static class PiggyBanksUtilities
{
    public static PiggyBank CreatePiggyBank()
        => new(
            new(Constants.Constants.PiggyBank.Id),
            new(Constants.Constants.PiggyBank.Name),
            Constants.Constants.PiggyBank.WithGoal,
            new(Constants.Constants.PiggyBank.Goal),
            new(Constants.Constants.User.Id)
        );
}