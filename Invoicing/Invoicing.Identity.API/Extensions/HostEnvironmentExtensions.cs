using Invoicing.Identity.API.Configuration;

namespace Invoicing.Identity.API.Extensions;

public static class HostEnvironmentExtensions
{
    public static bool IsSystemTests(this IHostEnvironment hostEnvironment)
    {
        return hostEnvironment.IsEnvironment(AppConfiguration.SystemTestsEnvironmentName);
    }
}