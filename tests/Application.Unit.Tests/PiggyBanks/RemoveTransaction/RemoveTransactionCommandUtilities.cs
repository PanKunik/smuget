using Application.PiggyBanks.RemoveTransaction;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.PiggyBanks.RemoveTransaction;

public static class RemoveTransactionCommandUtilities
{
    public static RemoveTransactionCommand CreateCommand()
        => new(
            Constants.PiggyBank.Id,
            Constants.Transaction.Id,
            Constants.User.Id
        );
}