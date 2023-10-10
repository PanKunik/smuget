using Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

internal sealed class SmugetDbContext : DbContext
{
    public SmugetDbContext(DbContextOptions<SmugetDbContext> options)
        : base(options) { }

    public DbSet<MonthlyBillingEntity> MonthlyBillings { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(SmugetDbContext).Assembly);
    }
}