#region Using

using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

#endregion Using

namespace MicroAutomation.Licensing.Data.Shared.DbContexts;

/// <summary>
/// A database instance represents a session with the data protection database.
/// </summary>
/// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
/// <seealso cref="Microsoft.AspNetCore.DataProtection.EntityFrameworkCore.IDataProtectionKeyContext" />
public class DataProtectionDbContext : DbContext, IDataProtectionKeyContext
{
    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataProtectionDbContext"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public DataProtectionDbContext(DbContextOptions<DataProtectionDbContext> options)
        : base(options) { }
}