using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;

internal sealed class SmugetDbContext : DbContext
{
    public SmugetDbContext(DbContextOptions<SmugetDbContext> options)
        : base(options) { }
}