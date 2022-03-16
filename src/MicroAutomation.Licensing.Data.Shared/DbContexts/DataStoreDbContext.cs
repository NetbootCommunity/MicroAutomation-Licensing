#region Using

using MicroAutomation.Licensing.Data.Configuration.Builders;
using MicroAutomation.Licensing.Data.Entities;
using Microsoft.EntityFrameworkCore;

#endregion Using

namespace MicroAutomation.Licensing.Data.Shared.DbContexts
{
    public class DataStoreDbContext : DbContext
    {
        public DataStoreDbContext(DbContextOptions<DataStoreDbContext> options)
           : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new LicenseEntityBuilder());
            builder.ApplyConfiguration(new ProductEntityBuilder());
            builder.ApplyConfiguration(new OrganizationEntityBuilder());
            builder.ApplyConfiguration(new UserEntityBuilder());
            builder.ApplyConfiguration(new UserOrganizationEntityBuilder());
        }

        public DbSet<LicenseEntity> Licenses { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<OrganizationEntity> Organizations { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<UserOrganizationEntity> UserOrganizations { get; set; }
    }
}