using Application.Abstractions.CQRS;
using Application.Abstractions.Persistance;
using Application.Exceptions;
using Domain.MonthlyBillings;
using Microsoft.EntityFrameworkCore;

namespace Application.MonthlyBillings.Commands.OpenMonthlyBilling;

public sealed class OpenMonthlyBillingCommandHandler : ICommandHandler<OpenMonthlyBillingCommand>
{
    private readonly ISmugetDbContext _dbContext;

    public OpenMonthlyBillingCommandHandler(ISmugetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(OpenMonthlyBillingCommand command, CancellationToken cancellationToken = default)
    {
        var year = new Year(command.Year);
        var month = new Month(command.Month);
        var currency = new Currency(command.Currency);

        var existingMonthlyBilling = await _dbContext.MonthlyBillings.FirstOrDefaultAsync(
            m => m.Year == year
            && m.Month == month,
            cancellationToken);

        if (existingMonthlyBilling is not null)
        {
            throw new MonthlyBillingAlreadyOpenedException(
                month.Value,
                year.Value);
        }

        var monthlyBilling = new MonthlyBilling(
            year,
            month,
            currency,
            State.Open,
            null,
            null
        );

        await _dbContext.MonthlyBillings.AddAsync(
            monthlyBilling,
            cancellationToken
        );

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}