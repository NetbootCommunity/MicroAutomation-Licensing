#region Using

using MicroAutomation.Licensing.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion Using

namespace MicroAutomation.Licensing.Data.Configuration.Builders;

public class UserEntityBuilder : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable(name: "Users");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Email)
            .IsUnique();

        builder.Property(x => x.Email)
            .HasMaxLength(64)
            .IsRequired();
        builder.Property(x => x.Email)
            .HasMaxLength(64)
            .IsRequired();
        builder.Property(x => x.DisplayName)
            .HasMaxLength(64)
            .IsRequired();
        builder.Property(x => x.CreatedDataUtc)
            .IsRequired();
    }
}