using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class InvalidMonthlyBillingIdException
    : RequiredFieldException
{
    public InvalidMonthlyBillingIdException()
        : base(nameof(MonthlyBillingId)) { }
}