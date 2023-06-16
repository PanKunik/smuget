using Domain.Exceptions;

namespace Application.Exceptions;

public sealed class MonthlyBillingAlreadyOpenedException : SmugetException
{
    public MonthlyBillingAlreadyOpenedException(byte month, ushort year)
        : base($"You already have opened monthly billing for `{month}/{year}`.") { }
}