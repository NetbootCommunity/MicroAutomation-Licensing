#region Using

using MicroAutomation.Licensing.Data.Configuration.Configuration;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

#endregion Using

namespace MicroAutomation.Licensing.Data.Shared.Helpers;

/// <summary>
/// Helper to easily migrate the database.
/// </summary>
public static class DbMigrationHelpers
{
    /// <summary>
    /// Apply all database migrations.
    /// </summary>
    /// <typeparam name="TDataStoreDbContext"></typeparam>
    /// <typeparam name="TDataProtectionDbContext"></typeparam>
    /// <param name="host"></param>
    /// <param name="databaseMigrationsConfiguration"></param>
    /// <returns></returns>
    public static async Task<bool> ApplyDbMigrationsAsync<TDataStoreDbContext, TDataProtectionDbContext>(IHost host, DatabaseMigrationsConfiguration databaseMigrationsConfiguration)
        where TDataStoreDbContext : DbContext
        where TDataProtectionDbContext : DbContext, IDataProtectionKeyContext
    {
        var migrationComplete = false;
        using (var serviceScope = host.Services.CreateScope())
        {
            var services = serviceScope.ServiceProvider;

            if (databaseMigrationsConfiguration?.ApplyDatabaseMigrations == true)
            {
                migrationComplete = await EnsureDatabasesMigratedAsync<TDataStoreDbContext, TDataProtectionDbContext>(services);
            }
        }
        return migrationComplete;
    }

    /// <summary>
    /// Ensure sure all databases are migrated.
    /// </summary>
    /// <typeparam name="TDataStoreDbContext"></typeparam>
    /// <typeparam name="TDataProtectionDbContext"></typeparam>
    /// <param name="services"></param>
    /// <returns></returns>
    public static async Task<bool> EnsureDatabasesMigratedAsync<TDataStoreDbContext, TDataProtectionDbContext>(IServiceProvider services)
        where TDataStoreDbContext : DbContext
        where TDataProtectionDbContext : DbContext, IDataProtectionKeyContext
    {
        int pendingMigrationCount = 0;
        using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            using (var context = scope.ServiceProvider.GetRequiredService<TDataStoreDbContext>())
            {
                await context.Database.MigrateAsync();
                pendingMigrationCount += (await context.Database.GetPendingMigrationsAsync()).Count();
            }

            using (var context = scope.ServiceProvider.GetRequiredService<TDataProtectionDbContext>())
            {
                await context.Database.MigrateAsync();
                pendingMigrationCount += (await context.Database.GetPendingMigrationsAsync()).Count();
            }
        }

        return pendingMigrationCount == 0;
    }
}