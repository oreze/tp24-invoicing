using System.Runtime.CompilerServices;

namespace Invoicing.Receivables.Infrastructure.Configuration;

public static class DbContextModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
}