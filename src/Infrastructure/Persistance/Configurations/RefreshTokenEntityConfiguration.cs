using Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

internal sealed class RefreshTokenEntityConfiguration : IEntityTypeConfiguration<RefreshTokenEntity>
{
    public void Configure(EntityTypeBuilder<RefreshTokenEntity> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .ToTable("RefreshTokens");

        builder
            .Property(e => e.Id)
            .IsRequired();

        builder
            .Property(e => e.Token)
            .IsRequired();

        builder
            .Property(e => e.Expires)
            .IsRequired();

        builder
            .Property(e => e.WasUsed)
            .IsRequired();

        builder
            .Property(e => e.UserId)
            .IsRequired();
    }
}
