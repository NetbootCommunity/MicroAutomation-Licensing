#region Using

using MicroAutomation.Licensing.Data.Configuration.Builders;
using MicroAutomation.Licensing.Data.Entities;
using MicroAutomation.Licensing.Data.Shared.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

#endregion Using

namespace MicroAutomation.Licensing.Data.Shared.DbContexts;

/// <summary>
/// A database instance represents a session with the data store database.
/// </summary>
/// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
public class DataStoreDbContext : DbContext
{
    /// <summary>
    /// Gets the context accessor.
    /// </summary>
    /// <value>The context accessor.</value>
    protected IHttpContextAccessor ContextAccessor { get; }

    /// <summary>
    /// Gets or sets the licenses.
    /// </summary>
    /// <value>
    /// The licenses.
    /// </value>
    public DbSet<LicenseEntity> Licenses { get; set; }

    /// <summary>
    /// Gets or sets the products.
    /// </summary>
    /// <value>
    /// The products.
    /// </value>
    public DbSet<ProductEntity> Products { get; set; }

    /// <summary>
    /// Gets or sets the organizations.
    /// </summary>
    /// <value>
    /// The organizations.
    /// </value>
    public DbSet<OrganizationEntity> Organizations { get; set; }

    /// <summary>
    /// Gets or sets the users.
    /// </summary>
    /// <value>
    /// The users.
    /// </value>
    public DbSet<UserEntity> Users { get; set; }

    /// <summary>
    /// Gets or sets the user organizations.
    /// </summary>
    /// <value>
    /// The user organizations.
    /// </value>
    public DbSet<UserOrganizationEntity> UserOrganizations { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataStoreDbContext"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    /// <param name="contextAccessor">The context accessor.</param>
    public DataStoreDbContext(DbContextOptions<DataStoreDbContext> options, IHttpContextAccessor contextAccessor)
       : base(options)
    {
        ContextAccessor = contextAccessor;
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new LicenseEntityBuilder());
        builder.ApplyConfiguration(new ProductEntityBuilder());
        builder.ApplyConfiguration(new OrganizationEntityBuilder());
        builder.ApplyConfiguration(new UserEntityBuilder());
        builder.ApplyConfiguration(new UserOrganizationEntityBuilder());
    }

    /// <summary>
    /// Saves all changes made in this context to the database
    /// </summary>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous save operation. The task result contains the
    /// number of state entries written to the database.
    /// </returns>
    /// <remarks>
    /// See <see href="https://aka.ms/efcore-docs-saving-data">Saving data in EF Core</see> for more information.
    /// </remarks>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.UpdateAuditableEntities(GetUserIdentifier());
        return base.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Gets the user identifier.
    /// </summary>
    /// <returns>The user identifier.</returns>
    /// <exception cref="System.ApplicationException"></exception>
    private Guid GetUserIdentifier()
    {
        // Validate if the user is authenticated
        var user = ContextAccessor.HttpContext?.User;
        if (user.Identity?.IsAuthenticated == true)
            throw new ApplicationException("Unable to retrieve user identifier, " +
                "please send the request with valid authentication");

        var stringIdentifier = user.FindFirst(ClaimTypes.NameIdentifier).Value;
        if (string.IsNullOrEmpty(stringIdentifier))
            throw new ApplicationException("The name identifier is null or empty, " +
                "please send the request with valid authentication");

        if (Guid.TryParse(stringIdentifier, out Guid identifier))
            return identifier;
        throw new ApplicationException("Unable to convert the name identifier to guid, " +
            "please send the request with valid authentication");
    }
}