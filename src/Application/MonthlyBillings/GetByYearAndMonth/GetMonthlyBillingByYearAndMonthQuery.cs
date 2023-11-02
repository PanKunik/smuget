using Application.Abstractions.CQRS;

namespace Application.MonthlyBillings.GetByYearAndMonth;

public sealed record GetMonthlyBillingByYearAndMonthQuery(
    ushort Year,
    byte Month,
    Guid UserId
) : IQuery<MonthlyBillingDTO>;