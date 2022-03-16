#region Using

using MicroAutomation.Licensing.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion Using

namespace MicroAutomation.Licensing.Data.Configuration.Builders;

public class OrganizationEntityBuilder : IEntityTypeConfiguration<OrganizationEntity>
{
    public void Configure(EntityTypeBuilder<OrganizationEntity> builder)
    {
        builder.ToTable(name: "Organizations");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.Name)
            .HasMaxLength(64)
            .IsRequired();
        builder.Property(x => x.Description)
            .HasMaxLength(128);

        builder.Property(x => x.CreatedDataBy)
            .IsRequired();
        builder.Property(x => x.CreatedDataUtc)
            .IsRequired();
    }
}