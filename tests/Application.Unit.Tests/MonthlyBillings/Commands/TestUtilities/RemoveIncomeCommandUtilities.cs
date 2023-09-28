using Application.MonthlyBillings.Commands.RemoveIncome;
using Application.Unit.Tests.TestUtilities.Constants;

namespace Application.Unit.Tests.MonthlyBillings.Commands.TestUtilities;

public static class RemoveIncomeCommandUtilities
{
    public static RemoveIncomeCommand CreateCommand()
        => new(
            Guid.NewGuid(),
            Constants.Income.Id
        );
}