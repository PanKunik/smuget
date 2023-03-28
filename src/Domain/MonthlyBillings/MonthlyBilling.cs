using Domain.Exceptions;
using Domain.MonthlyBillings.ValueObjects;
using Domain.Shared.Models;

namespace Domain.MonthlyBillings;

public sealed class MonthlyBilling : Entity<MonthlyBillingId>
{
    public Year Year { get; }
    public Month Month { get; }

    public MonthlyBilling(
        MonthlyBillingId id,
        Year year,
        Month month)
        : base(id)
    {
        if (id is null)
        {
            throw new MonthlyBillingIdIsNullException();
        }

        if (year is null)
        {
            throw new YearIsNullException();
        }

        Year = year;

        if (month is null)
        {
            throw new MonthIsNullException();
        }

        Month = month;
    }
}