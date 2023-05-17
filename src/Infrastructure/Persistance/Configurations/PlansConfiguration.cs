using Domain.MonthlyBillings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

internal sealed class PlansConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .Property(p => p.Id)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => new PlanId(value));

        builder
            .Property(p => p.Category)
            .IsRequired()
            .HasMaxLength(25)
            .HasConversion(
                category => category.Value,
                value => new Category(value));

        builder
            .OwnsOne(p => p.MoneyAmount);

        builder
            .Property(p => p.SortOrder)
            .IsRequired();
    }
}