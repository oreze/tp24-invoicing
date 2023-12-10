using Invoicing.Receivables.Infrastructure.Configuration;

namespace Invoicing.Receivables.Infrastructure.Extensions;

public static class HostEnvironmentExtensions
{
    public static bool IsSystemTests(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsEnvironment(AppConfiguration.SystemTestsEnvironmentName);
    }
}