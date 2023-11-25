using Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

internal sealed class PiggyBankEntityConfiguration
    : IEntityTypeConfiguration<PiggyBankEntity>
{
    public void Configure(EntityTypeBuilder<PiggyBankEntity> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .ToTable("PiggyBanks");

        builder
            .Property(p => p.Id)
            .IsRequired();

        builder
            .Property(p => p.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(p => p.WithGoal)
            .IsRequired();

        builder
            .Property(p => p.Goal)
            .IsRequired();

        builder
            .Property(p => p.UserId)
            .IsRequired();

        builder
            .HasMany(p => p.Transactions)
            .WithOne()
            .HasForeignKey(t => t.PiggyBankId)
            .IsRequired();
    }
}
