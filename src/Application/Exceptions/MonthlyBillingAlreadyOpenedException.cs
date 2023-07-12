using Domain.Exceptions;

namespace Application.Exceptions;

public sealed class MonthlyBillingAlreadyOpenedException : SmugetException
{
    public MonthlyBillingAlreadyOpenedException(int month, int year)
        : base($"You already have opened monthly billing for `{month}/{year}`.") { }
}