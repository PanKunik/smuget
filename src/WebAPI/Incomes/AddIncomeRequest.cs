namespace WebAPI.Incomes;

/// <summary>
/// Informations about new income for monthly billing.
/// </summary>
/// <param name="Name">Tag name for income.</param>
/// <param name="MoneyAmount">Amount of the income.</param>
/// <param name="Currency">Currency of the income.</param>
/// <param name="IncludeInBilling">Include in total amount of money available for planning?</param>
public sealed record AddIncomeRequest(
    string Name,
    decimal MoneyAmount,
    string Currency,
    bool IncludeInBilling = true);