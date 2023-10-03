using Application.Abstractions.CQRS;
using Application.MonthlyBillings;

namespace Application.MonthlyBillings.GetByYearAndMonth;

public sealed record GetMonthlyBillingByYearAndMonthQuery(
    ushort Year,
    byte Month
) : IQuery<MonthlyBillingDTO>;