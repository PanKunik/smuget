using Domain.Exceptions;

namespace Domain.MonthlyBillings;

public sealed class MonthlyBilling
{
    private MonthlyBilling() { }

    private readonly List<Income> _incomes = new();
    private readonly List<Plan> _plans = new();

    public MonthlyBillingId Id { get; } = new MonthlyBillingId(Guid.NewGuid());
    public Year Year { get; }
    public Month Month { get; }
    public State State { get; private set; } = State.Open;
    public IReadOnlyCollection<Income> Incomes => _incomes.AsReadOnly();
    public IReadOnlyCollection<Plan> Plans => _plans.AsReadOnly();

    public MonthlyBilling(
        Year year,
        Month month
    )
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

    public void AddIncome(Income income)
    {
        if (income is null)
        {
            throw new IncomeIsNullException();
        }

        if (_incomes.Any(i => i.Name.Equals(income.Name)))
        {
            throw new IncomeNameNotUniqueException(income.Name.Value);
        }

        _incomes.Add(income);
    }

    public void AddPlan(Plan plan)
    {
        if (plan is null)
        {
            throw new PlanIsNullException();
        }

        _plans.Add(plan);
    }

    public void Close()
    {
        if (State == State.Closed)
        {
            throw new MonthlyBillingAlreadyClosedException(Month, Year);
        }

        State = State.Closed;
    }
}