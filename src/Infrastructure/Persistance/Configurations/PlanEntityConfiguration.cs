using Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

internal sealed class PlanEntityConfiguration : IEntityTypeConfiguration<PlanEntity>
{
    public void Configure(EntityTypeBuilder<PlanEntity> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .ToTable("Plans");

        builder
            .Property(p => p.Id)
            .IsRequired();

        builder
            .Property(p => p.Category)
            .IsRequired()
            .HasMaxLength(25);

        builder
            .Property(p => p.MoneyAmount)
            .IsRequired();

        builder
            .Property(p => p.MoneyCurrency)
            .IsRequired();

        builder
            .Property(p => p.SortOrder)
            .IsRequired();

        builder
            .Property(p => p.Active)
            .IsRequired();

        builder
            .HasMany(p => p.Expenses)
            .WithOne()
            .HasForeignKey(e => e.PlanId)
            .IsRequired();

        builder
            .Property(p => p.MonthlyBillingId)
            .IsRequired();
    }
}