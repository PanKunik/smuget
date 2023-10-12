using Domain.Exceptions;
using Domain.Users;

namespace Domain.MonthlyBillings;

public sealed class MonthlyBilling
{
    private readonly List<Income> _incomes = new();
    private readonly List<Plan> _plans = new();

    public MonthlyBillingId Id { get; } = new MonthlyBillingId(Guid.NewGuid());
    public Year Year { get; }
    public Month Month { get; }
    public Currency Currency { get; }
    public State State { get; private set; } = State.Open;
    public UserId UserId { get; private set; }
    public IReadOnlyCollection<Income> Incomes => _incomes.AsReadOnly();
    public IReadOnlyCollection<Plan> Plans => _plans.AsReadOnly();

    public decimal SumOfIncome
        => _incomes?
            .Where(i => i.Active)
            .Sum(i => i.Money.Amount) ?? 0m;

    public decimal SumOfIncomeAvailableForPlanning
        => _incomes?
            .Where(i => i.Include && i.Active)
            .Sum(i => i.Money.Amount) ?? 0m;

    public decimal SumOfPlan
        => _plans?
            .Where(p => p.Active)
            .Sum(p => p.Money.Amount) ?? 0m;

    public decimal SumOfExpenses
        => _plans?
            .Where(p => p.Active)
            .Sum(p => p.SumOfExpenses) ?? 0m;

    public decimal AccountBalance
        => SumOfIncome - SumOfExpenses;

    public decimal SavingsForecast
        => SumOfIncome - SumOfPlan;

    public MonthlyBilling(
        MonthlyBillingId monthlyBillingId,
        Year year,
        Month month,
        Currency currency,
        State state,
        UserId userId,
        List<Plan>? plans = null,
        List<Income>? incomes = null
    )
    {
        ThrowIfMonthlyBillingIdIsNull(monthlyBillingId);
        ThrowIfYearIsNull(year);
        ThrowIfMonthIsNull(month);
        ThrowIfCurrencyIsNull(currency);
        ThrowIfUserIdIsNull(userId);

        Id = monthlyBillingId;
        Year = year;
        Month = month;
        Currency = currency;
        State = state;    // TODO: Check for null
        UserId = userId;

        _plans = plans ?? new();
        _incomes = incomes ?? new();
    }

    private void ThrowIfMonthlyBillingIdIsNull(MonthlyBillingId monthlyBillingId)
    {
        if (monthlyBillingId is null)
        {
            throw new MonthlyBillingIdIsNullException();
        }
    }

    private void ThrowIfYearIsNull(Year year)
    {
        if (year is null)
        {
            throw new YearIsNullException();
        }
    }

    private void ThrowIfMonthIsNull(Month month)
    {
        if (month is null)
        {
            throw new MonthIsNullException();
        }
    }

    private void ThrowIfCurrencyIsNull(Currency currency)
    {
        if (currency is null)
        {
            throw new CurrencyIsNullException();
        }
    }

    private void ThrowIfUserIdIsNull(UserId userId)
    {
        if (userId is null)
        {
            throw new UserIdIsNullException();
        }
    }

    public void AddIncome(Income income)
    {
        ThrowIfMonthlyBillingIsClosed();
        ThrowIfIncomeIsNull(income);
        ThrowIfIncomeNameNotUnique(income);
        ThrowIfCurrencyMismatched(income.Money.Currency);

        _incomes.Add(income);
    }

    private void ThrowIfMonthlyBillingIsClosed()
    {
        if (State == State.Closed)
        {
            throw new MonthlyBillingAlreadyClosedException(Month, Year);
        }
    }

    private void ThrowIfIncomeIsNull(Income income)
    {
        if (income is null)
        {
            throw new IncomeIsNullException();
        }
    }

    private void ThrowIfIncomeNameNotUnique(Income income)
    {
        if (_incomes.Any(i => i.Name.Equals(income.Name)))
        {
            throw new IncomeNameNotUniqueException(income.Name.Value);
        }
    }

    private void ThrowIfCurrencyMismatched(Currency currency)
    {
        if (currency != Currency)
        {
            throw new MonthlyBillingCurrencyMismatchException(currency);
        }
    }

    public void UpdateIncome(
        IncomeId incomeId,
        Name name,
        Money money,
        bool include
    )
    {
        ThrowIfIncomeIdIsNull(incomeId);
        ThrowIfMonthlyBillingIsClosed();

        var incomeToUpdate = _incomes?.Find(i => i.Id == incomeId && i.Active)
            ?? throw new IncomeNotFoundException();

        incomeToUpdate.Update(
            name,
            money,
            include
        );
    }

    private void ThrowIfIncomeIdIsNull(IncomeId incomeId)
    {
        if (incomeId is null)
        {
            throw new IncomeIdIsNullException();
        }
    }

    public void RemoveIncome(
        IncomeId incomeId
    )
    {
        ThrowIfMonthlyBillingIsClosed();
        ThrowIfIncomeIdIsNull(incomeId);

        var incomeToRemove = _incomes?.Find(i => i.Id == incomeId && i.Active)
            ?? throw new IncomeNotFoundException();

        incomeToRemove.Remove();
    }

    public void AddPlan(Plan plan)
    {
        ThrowIfMonthlyBillingIsClosed();
        ThrowIfPlanIsNull(plan);
        ThrowIfPlanCategoryNotUnique(plan);
        ThrowIfCurrencyMismatched(plan.Money.Currency);

        _plans.Add(plan);
    }

    private void ThrowIfPlanIsNull(Plan plan)
    {
        if (plan is null)
        {
            throw new PlanIsNullException();
        }
    }

    private void ThrowIfPlanCategoryNotUnique(Plan plan)
    {
        if (_plans.Any(p => p.Category.Equals(plan.Category)))
        {
            throw new PlanCategoryNotUniqueException(plan.Category.Value);
        }
    }

    public void UpdatePlan(
        PlanId planId,
        Category category,
        Money money,
        uint sortOrder
    )
    {
        ThrowIfMonthlyBillingIsClosed();
        ThrowIfPlanIdIsNull(planId);

        var planToUpdate = _plans?.Find(p => p.Id == planId && p.Active)
            ?? throw new PlanNotFoundException();

        planToUpdate.Update(
            category,
            money,
            sortOrder
        );
    }

    private void ThrowIfPlanIdIsNull(PlanId planId)
    {
        if (planId is null)
        {
            throw new PlanIdIsNullException();
        }
    }

    public void RemovePlan(
        PlanId planId
    )
    {
        ThrowIfMonthlyBillingIsClosed();
        ThrowIfPlanIdIsNull(planId);

        var planToRemove = _plans?.Find(i => i.Id == planId && i.Active)
            ?? throw new PlanNotFoundException();

        planToRemove.Remove();
    }

    public void AddExpense(
        PlanId planId,
        Expense expense
    )
    {
        ThrowIfMonthlyBillingIsClosed();
        ThrowIfPlanIdIsNull(planId);
        ThrowIfExpenseIsNull(expense);
        ThrowIfCurrencyMismatched(expense.Money.Currency);

        var plan = _plans?.Find(p => p.Id == planId && p.Active)
            ?? throw new PlanNotFoundException();

        plan.AddExpense(expense);
    }

    private void ThrowIfExpenseIsNull(Expense expense)
    {
        if (expense is null)
        {
            throw new ExpenseIsNullException();
        }
    }

    public void UpdateExpense(
        PlanId planId,
        ExpenseId expenseId,
        Money money,
        DateTimeOffset expenseDate,
        string description
    )
    {
        ThrowIfPlanIdIsNull(planId);

        var planToUpdateExpense = _plans?.Find(p => p.Id == planId && p.Active)
            ?? throw new PlanNotFoundException();

        planToUpdateExpense.UpdateExpense(
            expenseId,
            money,
            expenseDate,
            description
        );
    }

    public void RemoveExpense(
        PlanId planId,
        ExpenseId expenseId
    )
    {
        ThrowIfMonthlyBillingIsClosed();
        ThrowIfPlanIdIsNull(planId);

        var planToRemoveExpense = _plans?.Find(p => p.Id == planId && p.Active)
            ?? throw new PlanNotFoundException();

        planToRemoveExpense.RemoveExpense(expenseId);
    }

    public void Close()
    {
        ThrowIfMonthlyBillingIsClosed();
        State = State.Closed;
    }

    public void Reopen()
    {
        ThrowIfMonthlyBillingIsOpened();
        State = State.Open;
    }

    private void ThrowIfMonthlyBillingIsOpened()
    {
        if (State == State.Open)
        {
            throw new MonthlyBillingAlreadyOpenedException(Month, Year);
        }
    }
}