using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class MonthlyBillingAlreadyOpenedException
    : ConflictException
{
    public MonthlyBillingAlreadyOpenedException(
        Month month,
        Year year
    )
        : base(
            $"Monthly billing for `{month.Value}/{year.Value}` is already opened.",
            nameof(MonthlyBilling)
        ) { }
}