using Invoicing.Identity.Domain.Entities;
using Invoicing.Identity.Domain.Enums;

namespace Invoicing.Identity.API.Seeders;

public interface IDefaultEntitiesProvider
{
    public IEnumerable<(Roles role, IEnumerable<ApplicationUser> users)> DefaultUsers { get; }
}