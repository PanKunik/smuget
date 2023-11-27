using Application.PiggyBanks.AddTransaction;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.PiggyBanks.AddTransaction;

public static class AddTransactionCommandUtilities
{
    public static AddTransactionCommand CreateCommand()
        => new(
            Constants.PiggyBank.Id,
            Constants.Transaction.Value,
            Constants.Transaction.Date,
            Constants.User.Id
        );
}