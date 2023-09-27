using Application.MonthlyBillings.Commands.UpdateIncome;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.Commands.TestUtilities;

public static class UpdateIncomeCommandUtilities
{
    public static UpdateIncomeCommand CreateCommand()
        => new(
            Constants.MonthlyBilling.Id,
            Constants.Income.Id,
            "Updated Name Income",
            1234.56m,
            "EUR",
            false
        );
}