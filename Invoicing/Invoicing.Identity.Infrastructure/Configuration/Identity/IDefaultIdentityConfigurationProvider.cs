using Duende.IdentityServer.Models;

namespace Invoicing.Identity.API.Configuration.Interfaces.Interfaces;

public interface IDefaultIdentityConfigurationProvider
{
    public IEnumerable<IdentityResource> GetIdentityResources { get; }
    public IEnumerable<ApiScope> GetApiScopes { get; }
    public IEnumerable<Client> GetClients { get; }
}