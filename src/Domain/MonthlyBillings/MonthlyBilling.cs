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
    public Currency Currency { get; }
    public State State { get; private set; } = State.Open;
    public IReadOnlyCollection<Income> Incomes => _incomes.AsReadOnly();
    public IReadOnlyCollection<Plan> Plans => _plans.AsReadOnly();
    public decimal SumOfIncome
        => _incomes?.Sum(i => i.Money.Amount) ?? 0m;
    public decimal SumOfIncomeAvailableForPlanning
        => _incomes?
            .Where(i => i.Include)
            .Sum(i => i.Money.Amount) ?? 0m;

    public decimal SumOfPlan
        => _plans?.Sum(p => p.Money.Amount) ?? 0m;

    public decimal SumOfExpenses
        => _plans?.Sum(p => p.SumOfExpenses) ?? 0m;

    public decimal AccountBalance
        => SumOfIncome - SumOfExpenses;

    public MonthlyBilling(
        MonthlyBillingId monthlyBillingId,
        Year year,
        Month month,
        Currency currency,
        State state,
        List<Plan> plans = null,
        List<Income> incomes = null
    )
    {
        Id = monthlyBillingId;    // TODO: Check for null

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

        if (currency is null)
        {
            throw new CurrencyIsNullException();
        }

        Currency = currency;
        State = state;    // TODO: Check for null

        _plans = plans ?? new();
        _incomes = incomes ?? new();
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

        if (income.Money.Currency != Currency)
        {
            throw new MonthlyBillingCurrencyMismatchException(income.Money.Currency);
        }

        _incomes.Add(income);
    }

    public void AddPlan(Plan plan)
    {
        if (plan is null)
        {
            throw new PlanIsNullException();
        }

        if (_plans.Any(p => p.Category.Equals(plan.Category)))
        {
            throw new PlanCategoryNotUniqueException(plan.Category.Value);
        }

        if (plan.Money.Currency != Currency)
        {
            throw new MonthlyBillingCurrencyMismatchException(plan.Money.Currency);
        }

        _plans.Add(plan);
    }

    public void AddExpense(PlanId planId, Expense expense)
    {
        if (planId is null)
        {
            throw new PlanIdIsNullException();
        }

        if (expense is null)
        {
            throw new ExpenseIsNullException();
        }

        var plan = _plans.Find(p => p.Id == planId);

        if (plan is null)
        {
            throw new PlanNotFoundException(planId);
        }

        if (expense.Money.Currency != Currency)
        {
            throw new MonthlyBillingCurrencyMismatchException(expense.Money.Currency);
        }

        plan.AddExpense(expense);
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