namespace WebAPI.Transactions;

public sealed record UpdateTransactionRequest(
    decimal Value,
    DateOnly Date
);
