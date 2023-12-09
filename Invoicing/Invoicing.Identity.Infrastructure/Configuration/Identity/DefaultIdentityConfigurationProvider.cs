using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;
using Invoicing.Identity.API.Configuration.Interfaces.Interfaces;
using Microsoft.Extensions.Options;

namespace Invoicing.Identity.API.Configuration;

public class DefaultIdentityConfigurationProvider : IDefaultIdentityConfigurationProvider
{
    private static readonly ApiScope RolesApiScope = new("roles", "User roles");
    private static readonly ApiScope InvoicingApiScope = new("invoicing", "Adding nad reading receivables");
    private static readonly ApiScope StatisticsApiScope = new("statistics", "Access to statistics");

    private readonly IOptions<AppSettings> _appSettings;

    public DefaultIdentityConfigurationProvider(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
    }

    public IEnumerable<IdentityResource> GetIdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.Email(),
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new("roles", new[] { JwtClaimTypes.Role })
        };

    public IEnumerable<ApiScope> GetApiScopes =>
        new List<ApiScope>
        {
            InvoicingApiScope,
            StatisticsApiScope
        };

    public IEnumerable<Client> GetClients =>
        new List<Client>
        {
            // interactive ASP.NET Core Web App
            new()
            {
                ClientId = _appSettings.Value.DefaultOAuth2ClientID
                           ?? throw new ArgumentNullException(nameof(_appSettings.Value.DefaultOAuth2ClientID)),
                ClientSecrets = { new Secret(_appSettings.Value.DefaultOAuth2ClientSecret.Sha256()) },

                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Profile,
                    RolesApiScope.Name,
                    InvoicingApiScope.Name,
                    StatisticsApiScope.Name
                }
            }
        };
}