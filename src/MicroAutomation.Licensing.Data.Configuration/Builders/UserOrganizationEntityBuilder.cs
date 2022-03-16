#region Using

using MicroAutomation.Licensing.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion Using

namespace MicroAutomation.Licensing.Data.Configuration.Builders;

public class UserOrganizationEntityBuilder : IEntityTypeConfiguration<UserOrganizationEntity>
{
    public void Configure(EntityTypeBuilder<UserOrganizationEntity> builder)
    {
        builder.ToTable(name: "UserOrganizations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Role)
            .IsRequired();

        builder.Property(x => x.CreatedDataBy)
            .IsRequired();
        builder.Property(x => x.CreatedDataUtc)
            .IsRequired();

        builder.HasOne(x => x.Organization)
            .WithMany(x => x.UserOrganizations)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.User)
            .WithMany(x => x.UserOrganizations)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}