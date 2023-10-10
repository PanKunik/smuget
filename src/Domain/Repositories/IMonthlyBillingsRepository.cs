using Domain.MonthlyBillings;

namespace Domain.Repositories;

public interface IMonthlyBillingsRepository
{
    Task<MonthlyBilling?> Get(Year year, Month month);
    Task<MonthlyBilling?> GetById(MonthlyBillingId monthlyBillingId);
    Task Save(MonthlyBilling monthlyBilling);
}