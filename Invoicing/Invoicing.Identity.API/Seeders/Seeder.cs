using System.Security.Claims;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityModel;
using Invoicing.Identity.API.Configuration;
using Invoicing.Identity.API.Seeders;
using Invoicing.Identity.Domain.Entities;
using Invoicing.Identity.Domain.Enums;
using Invoicing.Identity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Invoicing.Identity.API.Seeders;

public class Seeder
{
    public static async Task EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            PersistedGrantDbContext persistedGrantDbContext = scope.ServiceProvider.GetService<PersistedGrantDbContext>() ??
                                              throw new ArgumentNullException(nameof(PersistedGrantDbContext));
            ConfigurationDbContext configurationDbContext = scope.ServiceProvider.GetService<ConfigurationDbContext>() ??
                                                          throw new ArgumentNullException(nameof(ConfigurationDbContext));
            ApplicationDbContext applicationDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>() ??
                                                        throw new ArgumentNullException(nameof(ApplicationDbContext));
            
            await persistedGrantDbContext.Database.EnsureDeletedAsync();
            await configurationDbContext.Database.EnsureDeletedAsync();
            await applicationDbContext.Database.EnsureDeletedAsync();
            await persistedGrantDbContext.Database.MigrateAsync();
            await configurationDbContext.Database.MigrateAsync();
            await applicationDbContext.Database.MigrateAsync();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            await SeedRoles(roleManager);
            await SeedUsers(userManager);
            await SeedConfig(configurationDbContext);
        }
    }

    private static async Task SeedConfig(ConfigurationDbContext configurationDbContext)
    {
        await AddIdentityClients(configurationDbContext);
        await AddIdentityApiScopes(configurationDbContext);
        await AddIdentityResources(configurationDbContext);
        
        await configurationDbContext.SaveChangesAsync();
    }

    private static async Task AddIdentityClients(ConfigurationDbContext configurationDbContext)
    {
        if (!configurationDbContext.Clients.Any())
        {
            await configurationDbContext.Clients.AddRangeAsync(
                IdentityConfig.Clients.Select(client => client.ToEntity()));
        }
    }
    
    private static async Task AddIdentityResources(ConfigurationDbContext configurationDbContext)
    {
        if (!configurationDbContext.IdentityResources.Any())
        {
            await configurationDbContext.IdentityResources.AddRangeAsync(
                IdentityConfig.IdentityResources.Select(res => res.ToEntity()));
        }
    }
    
    private static async Task AddIdentityApiScopes(ConfigurationDbContext configurationDbContext)
    {
        if (!configurationDbContext.ApiScopes.Any())
        {
            await configurationDbContext.ApiScopes.AddRangeAsync(
                IdentityConfig.ApiScopes.Select(apiScope => apiScope.ToEntity()));
        }
    }

    private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        IEnumerable<string> defaultRoles = Enum.GetNames(typeof(Roles));
        foreach (var role in defaultRoles)
        {
            var doesRoleExist = await roleManager.RoleExistsAsync(role);
            if (doesRoleExist)
                continue;

            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    private static async Task SeedUsers(UserManager<ApplicationUser> userManager)
    {
        foreach ((Roles role, IEnumerable<ApplicationUser> users) in DefaultEntities.DefaultUsers)
        {
            foreach (ApplicationUser applicationUser in users)
            {
                if (!await DoesUserExistAsync(userManager, applicationUser)) continue;

                await CreateUserAsync(userManager, applicationUser);
                await AssignToRoleAsync(userManager, applicationUser, role);
                await AddClaimsAsync(userManager, applicationUser,
                    new[] { new Claim(JwtClaimTypes.Role, role.ToString()) });

                Log.Debug($"User {applicationUser.UserName} created");
            }
        }
    }

    private static async Task<bool> DoesUserExistAsync(UserManager<ApplicationUser> userManager, ApplicationUser applicationUser)
    {
        var userInDatabase = await userManager.FindByNameAsync(applicationUser.UserName);

        if (userInDatabase != null)
        {
            Log.Debug($"User exists: {applicationUser.UserName}");
            return false;
        }
        return true;
    }

    private static async Task CreateUserAsync(UserManager<ApplicationUser> userManager, ApplicationUser applicationUser)
    {
        var createUserResult = await userManager.CreateAsync(applicationUser, "defaultPassword123!");

        if (!createUserResult.Succeeded) RaiseIdentityError(createUserResult);
    }

    private static async Task AssignToRoleAsync(UserManager<ApplicationUser> userManager, ApplicationUser applicationUser, Roles role)
    {
        var addRolesResult = await userManager.AddToRoleAsync(applicationUser, role.ToString());

        if (!addRolesResult.Succeeded) RaiseIdentityError(addRolesResult);
    }

    private static async Task AddClaimsAsync(UserManager<ApplicationUser> userManager, ApplicationUser applicationUser, params Claim[] claims)
    {
        var defaultClaims = new List<Claim>
        {
            new(JwtClaimTypes.Email, applicationUser.Email),
            new(JwtClaimTypes.Id, applicationUser.Id)
        }.Concat(claims);

        var addClaimsResult = await userManager.AddClaimsAsync(applicationUser, defaultClaims);
        if (!addClaimsResult.Succeeded) RaiseIdentityError(addClaimsResult);
    }

    private static void RaiseIdentityError(IdentityResult failedResult)
    {
        var message = $"Identity operation failed: {GetErrorsAsString(failedResult)}";
        Log.Error(message);
        throw new Exception(message);
    }

    private static string GetErrorsAsString(IdentityResult result)
    {
        return string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
    }
}
