using Application.Abstractions.Persistance;
using Domain.MonthlyBillings;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

internal sealed class SmugetDbContext : DbContext, ISmugetDbContext
{
    public SmugetDbContext(DbContextOptions<SmugetDbContext> options)
        : base(options) { }

    public DbSet<MonthlyBilling> MonthlyBillings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(SmugetDbContext).Assembly);
    }
}