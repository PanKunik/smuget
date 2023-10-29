using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class InvalidIncomeIdException
    : RequiredFieldException
{
    public InvalidIncomeIdException()
        : base(nameof(IncomeId)) { }
}