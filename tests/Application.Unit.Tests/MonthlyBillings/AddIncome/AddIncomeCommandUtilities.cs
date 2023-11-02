using Application.MonthlyBillings.AddIncome;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.TestUtilities;

public static class AddIncomeCommandUtilities
{
    public static AddIncomeCommand CreateCommand()
        => new(
            Constants.MonthlyBilling.Id,
            Constants.Income.Name,
            Constants.Income.Amount,
            Constants.Income.Currency,
            Constants.Income.Include,
            Constants.User.Id
        );
}