using Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

internal sealed class IncomeEntityConfiguration : IEntityTypeConfiguration<IncomeEntity>
{
    public void Configure(EntityTypeBuilder<IncomeEntity> builder)
    {
        builder
            .HasKey(i => i.Id);

        builder
            .ToTable("Incomes");

        builder
            .Property(i => i.Id)
            .IsRequired();

        builder
            .Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder
            .Property(i => i.MoneyAmount)
            .IsRequired();

        builder
            .Property(i => i.MoneyCurrency)
            .IsRequired();

        builder
            .Property(i => i.Include)
            .IsRequired();

        builder
            .Property(i => i.Active)
            .IsRequired();

        builder
            .Property(i => i.MonthlyBillingId)
            .IsRequired();
    }
}