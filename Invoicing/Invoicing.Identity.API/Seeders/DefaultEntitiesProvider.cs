using Invoicing.Identity.API.Configuration;
using Invoicing.Identity.Domain.Entities;
using Invoicing.Identity.Domain.Enums;
using Microsoft.Extensions.Options;

namespace Invoicing.Identity.API.Seeders;

public class DefaultEntitiesProvider : IDefaultEntitiesProvider
{
    private readonly IOptions<AppSettings> _appSettings;

    public DefaultEntitiesProvider(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings;
    }

    public IEnumerable<(Roles role, IEnumerable<ApplicationUser> users)> DefaultUsers =>
        new List<(Roles role, IEnumerable<ApplicationUser> users)>
        {
            (
                Roles.Administrator,
                new List<ApplicationUser>
                {
                    new()
                    {
                        UserName = _appSettings.Value.DefaultAdminUsername
                                   ?? throw new ArgumentNullException(nameof(_appSettings.Value.DefaultAdminUsername)),
                        Email = _appSettings.Value.DefaultAdminEmail
                                ?? throw new ArgumentNullException(nameof(_appSettings.Value.DefaultAdminEmail)),
                        EmailConfirmed = true
                    }
                }
            ),
            (
                Roles.Company,
                new List<ApplicationUser>
                {
                    new()
                    {
                        Company = new Company
                        {
                            CompanyName = "Random Company #1",
                            GlobalCompanyIdentifier = "12345678912345678912"
                        },
                        UserName = _appSettings.Value.DefaultCompanyUsername,
                        Email = _appSettings.Value.DefaultCompanyEmail,
                        EmailConfirmed = true
                    }
                }
            )
        };
}