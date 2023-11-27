namespace WebAPI.Transactions;

public sealed record AddTransactionRequest(
    decimal Value,
    DateOnly Date
);