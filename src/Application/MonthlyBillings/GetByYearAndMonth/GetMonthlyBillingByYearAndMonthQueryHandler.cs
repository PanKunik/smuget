using Application.Abstractions.CQRS;
using Application.Exceptions;
using Application.MonthlyBillings;
using Application.MonthlyBillings.Mappings;
using Domain.MonthlyBillings;
using Domain.Repositories;

namespace Application.MonthlyBillings.GetByYearAndMonth;

public sealed class GetMonthlyBillingByYearAndMonthQueryHandler : IQueryHandler<GetMonthlyBillingByYearAndMonthQuery, MonthlyBillingDTO>
{
    private readonly IMonthlyBillingRepository _repository;

    public GetMonthlyBillingByYearAndMonthQueryHandler(IMonthlyBillingRepository repository)
    {
        _repository = repository;
    }

    public async Task<MonthlyBillingDTO> HandleAsync(
        GetMonthlyBillingByYearAndMonthQuery query,
        CancellationToken token = default
    )
    {
        var monthlyBilling = await _repository.Get(
            new Year(query.Year),
            new Month(query.Month)
        ) ?? throw new MonthlyBillingNotFoundException(query.Year, query.Month);

        return monthlyBilling
            .ToDTO();
    }
}