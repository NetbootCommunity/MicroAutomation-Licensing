namespace MicroAutomation.Licensing.Data.Configuration.Configuration;

public class DatabaseMigrationsConfiguration
{
    public bool ApplyDatabaseMigrations { get; set; } = false;

    public string DataStoreDbMigrationsAssembly { get; private set; }
    public string DataProtectionDbMigrationsAssembly { get; private set; }

    public void SetMigrationsAssemblies(string commonMigrationsAssembly)
    {
        DataStoreDbMigrationsAssembly = commonMigrationsAssembly;
        DataProtectionDbMigrationsAssembly = commonMigrationsAssembly;
    }
}