using Domain.Exceptions;

namespace Application.Exceptions;

public sealed class MonthlyBillingNotFoundException : SmugetException
{
    public MonthlyBillingNotFoundException()
        : base("Monthly billing with passed id doesn't exist.") { }

    public MonthlyBillingNotFoundException(ushort year, byte month)
        : base($"There is no monthly billing for {month}/{year}") { }
}