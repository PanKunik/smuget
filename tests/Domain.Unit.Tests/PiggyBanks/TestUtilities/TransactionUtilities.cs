using Domain.PiggyBanks;

namespace Domain.Unit.Tests.PiggyBanks.TestUtilities;

public static class TransactionUtilities
{
    public static Transaction CreateTransaction()
        => new(
            Constants.Transaction.Id,
            Constants.Transaction.Value,
            Constants.Transaction.Date
        );
}