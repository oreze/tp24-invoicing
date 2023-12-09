using System.Security.Claims;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Invoicing.Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Invoicing.Identity.Infrastructure.Configuration.Identity;

public class CustomProfileService : IProfileService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomProfileService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        if (user != null)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Email, user.Email)
            };

            await AddRoleClaimsAsync(user, claims);

            claims.AddRange(context.Subject.Claims);

            context.IssuedClaims = claims;
        }
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var user = await _userManager.GetUserAsync(context.Subject);
        context.IsActive = user != null;
    }

    private async Task AddRoleClaimsAsync(ApplicationUser user, IList<Claim> claims)
    {
        var roles = await _userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            var claim = new Claim(ClaimTypes.Role, role);
            claims.Add(claim);
        }
    }
}
