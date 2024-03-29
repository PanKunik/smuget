using Domain.PiggyBanks;

namespace Application.Unit.Tests.TestUtilities;

public static class TransactionsUtilities
{
    public static IEnumerable<Transaction> CreateTransactions(
        int index = 2
    )
        => Enumerable.Range(0, index)
            .Select(r => new Transaction(
                new(Constants.Constants.Transaction.IdFromIndex(r)),
                Constants.Constants.Transaction.ValueFromIndex(r),
                Constants.Constants.Transaction.DateFromIndex(r)
            ))
            .ToList();
}