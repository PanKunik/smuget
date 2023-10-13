using Application.Abstractions.CQRS;
using Application.Exceptions;
using Application.MonthlyBillings.Mappings;
using Domain.Repositories;

namespace Application.MonthlyBillings.GetByYearAndMonth;

// TODO: Read model (CQRS) - implement faster data reading
public sealed class GetMonthlyBillingByYearAndMonthQueryHandler
    : IQueryHandler<GetMonthlyBillingByYearAndMonthQuery, MonthlyBillingDTO>
{
    private readonly IMonthlyBillingsRepository _repository;

    public GetMonthlyBillingByYearAndMonthQueryHandler(
        IMonthlyBillingsRepository repository
    )
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<MonthlyBillingDTO> HandleAsync(
        GetMonthlyBillingByYearAndMonthQuery query,
        CancellationToken token = default
    )
    {
        var entity = await _repository.Get(
            new(query.Year),
            new(query.Month),
            new(query.UserId)
        ) ?? throw new MonthlyBillingNotFoundException(
                query.Year,
                query.Month
            );

        return entity.ToDTO();
    }
}