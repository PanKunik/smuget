using Domain.MonthlyBillings;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Persistance;

public interface ISmugetDbContext
{
    public DbSet<MonthlyBilling> MonthlyBillings { get; set; }

    public Task<int> SaveChangesAsync(CancellationToken token = default);
}