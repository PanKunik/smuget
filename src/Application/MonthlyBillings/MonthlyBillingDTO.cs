namespace Application.MonthlyBillings;

public sealed class MonthlyBillingDTO
{
    public Guid Id { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public string State { get; set; } = null!;
    public IEnumerable<IncomeDTO?> Incomes { get; set; } = null!;
    public IEnumerable<PlanDTO?> Plans { get; set; } = null!;
    public decimal SumOfIncome { get; set; }
    public decimal SumOfIncomeAvailableForPlanning { get; set; }
    public decimal SumOfPlan { get; set; }
    public decimal SumOfExpenses { get; set; }
    public decimal AccountBalance { get; set; }
    public decimal SavingsForecast { get; set; }
}