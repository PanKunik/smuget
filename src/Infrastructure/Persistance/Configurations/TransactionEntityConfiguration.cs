using Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

internal sealed class TransactionEntityConfiguration
    : IEntityTypeConfiguration<TransactionEntity>
{
    public void Configure(EntityTypeBuilder<TransactionEntity> builder)
    {
        builder
            .HasKey(t => t.Id);

        builder
            .ToTable("Transactions");

        builder
            .Property(t => t.Id)
            .IsRequired();

        builder
            .Property(t => t.Value)
            .IsRequired();

        builder
            .Property(t => t.Date)
            .IsRequired();

        builder
            .Property(p => p.PiggyBankId)
            .IsRequired();
    }
}