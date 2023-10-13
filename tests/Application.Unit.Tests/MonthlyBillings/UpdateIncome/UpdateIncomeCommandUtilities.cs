using Application.MonthlyBillings.UpdateIncome;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.TestUtilities;

public static class UpdateIncomeCommandUtilities
{
    public static UpdateIncomeCommand CreateCommand()
        => new(
            Constants.MonthlyBilling.Id,
            Constants.Income.Id,
            "Updated Name Income",
            1234.56m,
            "EUR",
            false,
            Constants.User.Id
        );
}