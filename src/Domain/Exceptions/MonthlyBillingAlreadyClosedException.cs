using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class MonthlyBillingAlreadyClosedException : SmugetException
{
    public MonthlyBillingAlreadyClosedException(Month month, Year year)
        : base($"Monthly billing for `{month.Value}/{year.Value}` is already closed.") { }
}