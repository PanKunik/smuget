using Domain.Exceptions;
using Domain.MonthlyBillings;

namespace Application.Exceptions;

public sealed class MonthlyBillingAlreadyOpenedException
    : ConflictException
{
    public MonthlyBillingAlreadyOpenedException(
        int month,
        int year
    )
        : base(
            $"You already have opened monthly billing for `{month}/{year}`.",
            nameof(MonthlyBilling)
        ) { }
}