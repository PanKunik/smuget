using Domain.Exceptions;
using Domain.MonthlyBillings;

namespace Application.Exceptions;

public sealed class MonthlyBillingNotFoundException
    : NotFoundException
{
    public MonthlyBillingNotFoundException(
        ushort year,
        byte month
    )
        : base(
            $"There is no monthly billing for {month}/{year}",
            nameof(MonthlyBilling)
        ) { }

    public MonthlyBillingNotFoundException(MonthlyBillingId monthlyBillingId)
        : base(
            nameof(MonthlyBilling),
            nameof(MonthlyBillingId),
            monthlyBillingId.Value.ToString()
        ) { }
}