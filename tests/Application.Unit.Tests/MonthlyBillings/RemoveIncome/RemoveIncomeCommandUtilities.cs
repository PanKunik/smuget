using Application.MonthlyBillings.RemoveIncome;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.TestUtilities;

public static class RemoveIncomeCommandUtilities
{
    public static RemoveIncomeCommand CreateCommand()
        => new(
            Constants.MonthlyBilling.Id,
            Constants.Income.Id,
            Constants.User.Id
        );
}