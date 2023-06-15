using Application.Abstractions.CQRS;
using Application.Abstractions.Persistance;
using Application.Exceptions;
using Application.MonthlyBillings.DTO;
using Application.MonthlyBillings.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Application.MonthlyBillings.Queries.GetByYearAndMonth;

public sealed class GetMonthlyBillingByYearAndMonthQueryHandler : IQueryHandler<GetMonthlyBillingByYearAndMonthQuery, MonthlyBillingDTO>
{
    private readonly ISmugetDbContext _dbContext;

    public GetMonthlyBillingByYearAndMonthQueryHandler(ISmugetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MonthlyBillingDTO> HandleAsync(
        GetMonthlyBillingByYearAndMonthQuery query,
        CancellationToken token = default
    )
    {
        var monthlyBilling = await _dbContext.MonthlyBillings
            .Include(m => m.Incomes)
            .Include(m => m.Plans)
            .ThenInclude(p => p.Expenses)
            .FirstOrDefaultAsync(
                m => m.Year.Value == query.Year
                && m.Month.Value == query.Month,
                token
            ) ?? throw new MonthlyBillingNotFoundException(query.Year, query.Month);

        return monthlyBilling
            .ToDTO();
    }
}