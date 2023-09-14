using Domain.MonthlyBillings;

namespace Application.Unit.Tests.TestUtils;

public static class IncomeUtils
{
    public static Income CreateIncome()
        => new Income(
            new IncomeId(Constants.Constants.Income.Id),
            new Name(Constants.Constants.Income.Name),
            new Money(
                Constants.Constants.Income.Amount,
                new Currency(Constants.Constants.Income.Currency)
            ),
            Constants.Constants.Income.Include
        );
}