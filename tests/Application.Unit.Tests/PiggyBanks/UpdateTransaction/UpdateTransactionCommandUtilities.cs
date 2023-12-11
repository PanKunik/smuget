using Application.PiggyBanks.UpdateTransaction;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.PiggyBanks.UpdateTransaction;

public static class UpdateTransactionCommandUtilities
{
    public static UpdateTransactionCommand CreateCommand()
        => new(
            Constants.PiggyBank.Id,
            Constants.Transaction.Id,
            Constants.User.Id,
            777M,
            new DateOnly(2019, 12, 19)
        );
}
