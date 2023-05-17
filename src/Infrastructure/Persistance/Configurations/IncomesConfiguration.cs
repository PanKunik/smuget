using Domain.MonthlyBillings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

internal sealed class IncomesConfiguration : IEntityTypeConfiguration<Income>
{
    public void Configure(EntityTypeBuilder<Income> builder)
    {
        builder
            .HasKey(i => i.Id);

        builder
            .Property(i => i.Id)
            .IsRequired()
            .HasConversion(
                id => id.Value,
                value => new(value));

        builder
            .Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasConversion(
                name => name.Value,
                value => new(value));

        builder
            .OwnsOne(i => i.Money);

        builder
            .Property(i => i.Include)
            .IsRequired();
    }
}