using Domain.MonthlyBillings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

internal sealed class IncomesConfiguration : IEntityTypeConfiguration<Income>
{
    public void Configure(EntityTypeBuilder<Income> builder)
    {
        builder
            .HasKey(b => b.Id);

        builder
            .Property(b => b.Id)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => new(v));

        builder
            .Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion(
                v => v.Value,
                v => new(v));

        builder
            .OwnsOne<Money>(m => m.Money);

        builder
            .Property(b => b.Include)
            .IsRequired();
    }
}