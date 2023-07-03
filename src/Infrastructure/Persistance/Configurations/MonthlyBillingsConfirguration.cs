using Domain.MonthlyBillings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

internal sealed class MonthlyBillingsConfiguration : IEntityTypeConfiguration<MonthlyBilling>
{
    public void Configure(EntityTypeBuilder<MonthlyBilling> builder)
    {
        builder
            .HasKey(k => k.Id);

        builder
            .Property(b => b.Id)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => new(v));

        builder
            .Property(b => b.Year)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => new(v));

        builder
            .Property(b => b.Month)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => new(v));

        builder
            .Property(b => b.Currency)
            .IsRequired()
            .HasConversion(
                v => v.Value,
                v => new(v));
    }
}
