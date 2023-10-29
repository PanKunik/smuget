using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class IncomeNotFoundException
    : NotFoundException
{
    public IncomeNotFoundException(IncomeId incomeId)
        : base(
            nameof(Income),
            nameof(IncomeId),
            incomeId.Value.ToString()
        ) { }
}