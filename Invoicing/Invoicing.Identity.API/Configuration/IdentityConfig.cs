using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Invoicing.Identity.API.Configuration;

public static class IdentityConfig
{
    private static readonly ApiScope InvoicingApiScope = new ApiScope("invoicing", "Adding nad reading receivables");
    private static readonly ApiScope StatisticsApiScope = new ApiScope("statistics", "Access to statistics");
    
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>()
        {
            new IdentityResources.Email(),
            new IdentityResources.OpenId()
        };
    
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            InvoicingApiScope,
            StatisticsApiScope
        };
    
    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            // interactive ASP.NET Core Web App
            new Client
            {
                ClientId = "client",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Profile,
                    InvoicingApiScope.Name,
                    StatisticsApiScope.Name
                }
            }
        };
}
