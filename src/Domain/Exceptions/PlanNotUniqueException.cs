using Domain.MonthlyBillings;

namespace Domain.Exceptions;

public sealed class PlanCategoryNotUniqueException
    : ConflictException
{

    public PlanCategoryNotUniqueException(
        string key,
        string keyValue
    )
        : base(
            nameof(Plan),
            key,
            keyValue
        ) { }

}