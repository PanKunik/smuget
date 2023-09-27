using Domain.MonthlyBillings;

namespace Application.Unit.Tests.TestUtilities;

public static class IncomesUtilities
{
    public static Income CreateIncome()
        => new(
            new(Constants.Constants.Income.Id),
            new(Constants.Constants.Income.Name),
            new(
                Constants.Constants.Income.Amount,
                new(Constants.Constants.Income.Currency)
            ),
            Constants.Constants.Income.Include
        );
}