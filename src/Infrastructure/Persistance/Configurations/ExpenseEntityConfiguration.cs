using Domain.MonthlyBillings;
using Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

internal sealed class ExpenseEntityConfiguration : IEntityTypeConfiguration<ExpenseEntity>
{
    public void Configure(EntityTypeBuilder<ExpenseEntity> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .ToTable("Expenses");

        builder
            .Property(e => e.Id)
            .IsRequired();

        builder
            .Property(e => e.MoneyAmount)
            .IsRequired();

        builder
            .Property(e => e.MoneyCurrency)
            .IsRequired();

        builder
            .Property(e => e.Description)
            .IsRequired(false)
            .HasMaxLength(50);

        builder
            .Property(e => e.ExpenseDate)
            .IsRequired();

        builder
            .Property(e => e.PlanId)
            .IsRequired();
    }
}
