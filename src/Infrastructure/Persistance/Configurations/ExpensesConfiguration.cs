using Domain.MonthlyBillings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

internal sealed class ExpensesConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .Property(e => e.Id)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => new ExpenseId(value));

        builder
            .OwnsOne(e => e.Money)
            .Property(m => m.Amount)
            .IsRequired();

        builder
            .OwnsOne(e => e.Money)
            .Property(m => m.Currency)
            .HasConversion(
                currency => currency.Value,
                value => new(value)
            )
            .IsRequired();

        builder
            .Property(e => e.Descritpion)
            .IsRequired(false)
            .HasMaxLength(50);

        builder
            .Property(e => e.ExpenseDate)
            .IsRequired();
    }
}
