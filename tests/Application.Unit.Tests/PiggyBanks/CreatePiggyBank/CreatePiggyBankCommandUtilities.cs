using Application.PiggyBanks.CreatePiggyBank;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.PiggyBanks.CreatePiggyBank;

public static class CreatePiggyBankCommandUtilities
{
    public static CreatePiggyBankCommand CreateCommand()
        => new(
            "New piggybank name",
            Constants.PiggyBank.WithGoal,
            Constants.PiggyBank.Goal,
            Constants.User.Id
        );
}