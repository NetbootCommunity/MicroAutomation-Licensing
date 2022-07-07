#region Using

using MicroAutomation.Licensing.Api.Service.Configurations;
using MicroAutomation.Licensing.Data.Configuration.Configuration;
using MicroAutomation.Licensing.Data.Shared.DbContexts;
using MicroAutomation.Licensing.Data.Shared.Helpers;
using MicroAutomation.Log.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

#endregion Using

namespace MicroAutomation.Licensing.Api.Service;

public class Program
{
    private const string MigrateOnlyArgs = "/migrateonly";

    /// <summary>
    /// Main entry point to run this application.
    /// </summary>
    /// <param name="args"></param>
    public static async Task Main(string[] args)
    {
        var configuration = GetConfiguration(args).Build();

        Serilog.Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        try
        {
            var host = CreateHostBuilder(args).Build();

            var migrationComplete = await ApplyDbMigrationsAsync(configuration, host);
            if (args.Any(x => x == MigrateOnlyArgs))
            {
                await host.StopAsync();
                if (!migrationComplete)
                {
                    Environment.ExitCode = -1;
                }

                return;
            }

            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Serilog.Log.Fatal(ex, "Host terminated unexpectedly");
        }
        finally
        {
            Serilog.Log.CloseAndFlush();
        }
    }

    /// <summary>
    /// Apply database migration.
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="host"></param>
    /// <returns></returns>
    private static async Task<bool> ApplyDbMigrationsAsync(IConfiguration configuration, IHost host)
    {
        var testingConfiguration = configuration.GetSection(nameof(TestingConfiguration)).Get<TestingConfiguration>();
        if (testingConfiguration?.IsStaging == false)
        {
            var databaseMigrationsConfiguration = configuration.GetSection(nameof(DatabaseMigrationsConfiguration))
                .Get<DatabaseMigrationsConfiguration>();

            return await DbMigrationHelpers
                .ApplyDbMigrationsAsync<DataStoreDbContext, DataProtectionDbContext>(host, databaseMigrationsConfiguration);
        }
        return true;
    }

    /// <summary>
    /// Create host builder.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);

        // Set current directory
        //var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
        //var pathContextRoot = new FileInfo(location.AbsolutePath).Directory.FullName;
        //builder.UseContentRoot(pathContextRoot);
        //Directory.SetCurrentDirectory(pathContextRoot);

        // Add serilog implementation
        builder.UseCustomSerilog();

        // Sets up the configuration for the remainder of the build process and application
        builder.ConfigureAppConfiguration((_, config) => config = GetConfiguration(args));

        // Configures a HostBuilder with defaults for hosting a web app.
        builder.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

        return builder;
    }

    /// <summary>
    /// Build and return product configuration.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    private static IConfigurationBuilder GetConfiguration(string[] args)
    {
        // Retrieve the name of the environment
        var aspnetcore = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        var dotnetcore = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        var environment = string.IsNullOrWhiteSpace(aspnetcore) ? dotnetcore : aspnetcore;
        if (string.IsNullOrWhiteSpace(environment))
            environment = "Production";
        var isDevelopment = environment == Environments.Development;
        var configurationBuilder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddJsonFile("serilog.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"serilog.{environment}.json", optional: true, reloadOnChange: true);

        if (isDevelopment)
            configurationBuilder.AddUserSecrets<Startup>(true);

        configurationBuilder.AddCommandLine(args);
        configurationBuilder.AddEnvironmentVariables();

        return configurationBuilder;
    }
}