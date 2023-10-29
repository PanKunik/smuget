using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class MonthlyBillingIdIsNullException
    : RequiredFieldException
{
    public MonthlyBillingIdIsNullException()
        : base(nameof(MonthlyBillingId)) { }
}