using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class CurrencyIsNullException
    : RequiredFieldException
{
    public CurrencyIsNullException()
        : base(nameof(Currency)) { }
}