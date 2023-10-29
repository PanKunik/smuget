using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class IncomeIdIsNullException
    : RequiredFieldException
{
    public IncomeIdIsNullException()
        : base(nameof(IncomeId)) { }
}