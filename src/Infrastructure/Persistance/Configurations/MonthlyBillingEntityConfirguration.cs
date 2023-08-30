using Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

internal sealed class MonthlyBillingEntityConfiguration : IEntityTypeConfiguration<MonthlyBillingEntity>
{
    public void Configure(EntityTypeBuilder<MonthlyBillingEntity> builder)
    {
        builder
            .HasKey(k => k.Id);

        builder
            .ToTable("MonthlyBillings");

        builder
            .Property(b => b.Id)
            .IsRequired();

        builder
            .Property(b => b.Year)
            .IsRequired();

        builder
            .Property(b => b.Month)
            .IsRequired();

        builder
            .Property(b => b.Currency)
            .IsRequired();

        builder
            .Property(b => b.State)
            .IsRequired();

        builder
            .HasMany(b => b.Incomes)
            .WithOne()
            .HasForeignKey(i => i.MonthlyBillingId)
            .IsRequired();

        builder
            .HasMany(b => b.Plans)
            .WithOne()
            .HasForeignKey(p => p.MonthlyBillingId)
            .IsRequired();
    }
}
