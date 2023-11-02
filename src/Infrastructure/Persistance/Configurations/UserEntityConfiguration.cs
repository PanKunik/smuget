using Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;

internal sealed class UserEntityConfiguration
    : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder
            .HasKey(e => e.Id);

        builder
            .ToTable("Users");

        builder
            .Property(e => e.Id)
            .IsRequired();

        builder
            .Property(e => e.Email)
            .HasMaxLength(60)
            .IsRequired();

        builder
            .Property(e => e.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(e => e.SecuredPassword)
            .HasMaxLength(200)
            .IsRequired();
    }
}
