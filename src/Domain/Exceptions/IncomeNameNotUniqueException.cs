using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class IncomeNameNotUniqueException
    : ConflictException
{
    public IncomeNameNotUniqueException(
        string key,
        string keyValue
    )
        : base(
            nameof(Income),
            key,
            keyValue
        ) { }
}