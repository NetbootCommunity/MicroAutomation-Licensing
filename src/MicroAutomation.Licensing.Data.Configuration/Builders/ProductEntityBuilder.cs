#region Using

using MicroAutomation.Licensing.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

#endregion Using

namespace MicroAutomation.Licensing.Data.Configuration.Builders;

public class ProductEntityBuilder : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.ToTable(name: "Products");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Name)
            .IsUnique();

        builder.Property(x => x.Name)
            .HasMaxLength(64)
            .IsRequired();
        builder.Property(x => x.Description)
            .HasMaxLength(128);
        builder.Property(x => x.PassPhrase)
            .IsRequired();
        builder.Property(x => x.PrivateKey)
            .IsRequired();
        builder.Property(x => x.PublicKey)
            .IsRequired();

        builder.Property(x => x.CreatedDataBy)
            .IsRequired();
        builder.Property(x => x.CreatedDataUtc)
            .IsRequired();

        builder.HasOne(x => x.Organization)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}