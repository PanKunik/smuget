using Application.Abstractions.Repositories;

namespace Infrastructure.Persistance.Repositories;

internal sealed class MonthlyBillingsRepository : IMonthlyBillingsRepository
{
    private readonly SmugetDbContext _dbContext;

    public MonthlyBillingsRepository(SmugetDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add()
    {
        
    }
}