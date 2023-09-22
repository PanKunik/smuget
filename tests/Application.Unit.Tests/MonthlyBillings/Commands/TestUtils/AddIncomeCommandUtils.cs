using Application.MonthlyBillings.Commands.AddIncome;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.Commands.TestUtils;

public static class AddIncomeCommandUtils
{
    public static AddIncomeCommand CreateCommand()
        => new AddIncomeCommand(
            Constants.MonthlyBilling.Id,
            Constants.Income.Name,
            Constants.Income.Amount,
            Constants.Income.Currency,
            Constants.Income.Include
        );
}