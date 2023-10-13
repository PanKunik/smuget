using Domain.MonthlyBillings;
using Domain.Users;

namespace Domain.Repositories;

public interface IMonthlyBillingsRepository
{
    Task<MonthlyBilling?> Get(Year year, Month month, UserId userId);
    Task<MonthlyBilling?> GetById(MonthlyBillingId monthlyBillingId, UserId userId);
    Task Save(MonthlyBilling monthlyBilling);
}