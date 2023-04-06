using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed class MonthlyBilling
{
    public MonthlyBillingId Id { get; } = new MonthlyBillingId(Guid.NewGuid());
    public Year Year { get; }
    public Month Month { get; }

    public MonthlyBilling(
        Year year,
        Month month)
    {
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

    private MonthlyBilling() { }
}