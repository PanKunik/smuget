using Application.Abstractions.CQRS;
using Application.MonthlyBillings.DTO;

namespace Application.MonthlyBillings.Queries.GetByYearAndMonth;

public sealed record GetMonthlyBillingByYearAndMonthQuery(
    ushort Year,
    byte Month
) : IQuery<MonthlyBillingDTO>;