using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class IncomeIsNullException
    : RequiredFieldException
{
    public IncomeIsNullException()
        : base(nameof(Income)) { }
}