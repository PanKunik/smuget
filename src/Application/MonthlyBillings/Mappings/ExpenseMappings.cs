using Application.MonthlyBillings.DTO;
using Domain.MonthlyBillings;

namespace Application.MonthlyBillings.Mappings;

public static class ExpenseMappings
{
    public static ExpenseDTO ToDTO(this Expense domain)
    {
        if (domain is null)
        {
            return null;
        }

        return new ExpenseDTO()
        {
            Id = domain.Id.Value.ToString(),
            Money = domain.Money.ToString(),
            ExpenseDate = domain.ExpenseDate,
            Description = domain.Description
        };
    }
}