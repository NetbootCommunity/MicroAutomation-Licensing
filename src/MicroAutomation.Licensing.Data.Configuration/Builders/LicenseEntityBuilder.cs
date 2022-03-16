#region Using

using MicroAutomation.Licensing.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using System.Text.Json;

#endregion Using

namespace MicroAutomation.Licensing.Data.Configuration.Builders;

public class LicenseEntityBuilder : IEntityTypeConfiguration<LicenseEntity>
{
    public void Configure(EntityTypeBuilder<LicenseEntity> builder)
    {
        builder.ToTable(name: "Licenses");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(64)
            .IsRequired();
        builder.Property(x => x.Email)
            .HasMaxLength(64)
            .IsRequired();
        builder.Property(x => x.Company)
            .HasMaxLength(64);
        builder.Property(x => x.Type)
            .IsRequired();
        builder.Property(u => u.AdditionalAttributes)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null))
            .HasMaxLength(5000);
        builder.Property(u => u.ProductFeatures)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null))
            .HasMaxLength(5000);

        builder.Property(x => x.CreatedDataBy)
            .IsRequired();
        builder.Property(x => x.CreatedDataUtc)
            .IsRequired();

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Licenses)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}