using System.Security.Claims;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityModel;
using Invoicing.Identity.API.Configuration;
using Invoicing.Identity.API.Configuration.Interfaces.Interfaces;
using Invoicing.Identity.API.Extensions;
using Invoicing.Identity.Domain.Entities;
using Invoicing.Identity.Domain.Enums;
using Invoicing.Identity.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

namespace Invoicing.Identity.API.Seeders;

public class DbSeeder : IDbSeeder
{
    private readonly IOptions<AppSettings> _appSettings;

    public DbSeeder(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings ?? throw new ArgumentException(nameof(appSettings));
    }

    public async Task EnsureSeedData(WebApplication app)
    {
        using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var persistedGrantDbContext = scope.ServiceProvider.GetService<PersistedGrantDbContext>() ??
                                          throw new ArgumentNullException(nameof(PersistedGrantDbContext));
            var configurationDbContext = scope.ServiceProvider.GetService<ConfigurationDbContext>() ??
                                         throw new ArgumentNullException(nameof(ConfigurationDbContext));
            var applicationDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>() ??
                                       throw new ArgumentNullException(nameof(ApplicationDbContext));

            if (app.Environment.IsSystemTests())
            {
                await persistedGrantDbContext.Database.EnsureDeletedAsync();
                await configurationDbContext.Database.EnsureDeletedAsync();
                await applicationDbContext.Database.EnsureDeletedAsync();
            }

            await persistedGrantDbContext.Database.MigrateAsync();
            await configurationDbContext.Database.MigrateAsync();
            await applicationDbContext.Database.MigrateAsync();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var defaultEntitiesProvider = scope.ServiceProvider.GetRequiredService<IDefaultEntitiesProvider>();
            var defaultIdentityConfigProvider = scope.ServiceProvider.GetRequiredService<IDefaultIdentityConfigurationProvider>();

            await SeedRoles(roleManager);
            await SeedUsers(userManager, defaultEntitiesProvider);
            await SeedConfig(configurationDbContext, defaultIdentityConfigProvider);

            await persistedGrantDbContext.SaveChangesAsync();
            await configurationDbContext.SaveChangesAsync();
            await applicationDbContext.SaveChangesAsync();
        }
    }

    private async Task SeedConfig(ConfigurationDbContext configurationDbContext,
        IDefaultIdentityConfigurationProvider defaultIdentityConfigurationProvider)
    {
        await AddIdentityClients(configurationDbContext, defaultIdentityConfigurationProvider);
        await AddIdentityApiScopes(configurationDbContext, defaultIdentityConfigurationProvider);
        await AddIdentityResources(configurationDbContext, defaultIdentityConfigurationProvider);
    }

    private async Task AddIdentityClients(ConfigurationDbContext configurationDbContext,
        IDefaultIdentityConfigurationProvider identityConfigurationProvider)
    {
        if (!configurationDbContext.Clients.Any())
            await configurationDbContext.Clients.AddRangeAsync(
                identityConfigurationProvider.GetClients.Select(client => client.ToEntity()));
    }

    private async Task AddIdentityResources(ConfigurationDbContext configurationDbContext,
        IDefaultIdentityConfigurationProvider identityConfigurationProvider)
    {
        if (!configurationDbContext.IdentityResources.Any())
            await configurationDbContext.IdentityResources.AddRangeAsync(
                identityConfigurationProvider.GetIdentityResources.Select(res => res.ToEntity()));
    }

    private async Task AddIdentityApiScopes(ConfigurationDbContext configurationDbContext,
        IDefaultIdentityConfigurationProvider identityConfigurationProvider)
    {
        if (!configurationDbContext.ApiScopes.Any())
            await configurationDbContext.ApiScopes.AddRangeAsync(
                identityConfigurationProvider.GetApiScopes.Select(apiScope => apiScope.ToEntity()));
    }

    private async Task SeedRoles(RoleManager<IdentityRole> roleManager)
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

    private async Task SeedUsers(UserManager<ApplicationUser> userManager, IDefaultEntitiesProvider defaultEntitiesProvider)
    {
        foreach (var (role, users) in defaultEntitiesProvider.DefaultUsers)
        foreach (var applicationUser in users)
        {
            if (!await DoesUserExistAsync(userManager, applicationUser)) continue;

            await CreateUserAsync(userManager, applicationUser);
            await AssignToRoleAsync(userManager, applicationUser, role);
            await AddClaimsAsync(userManager, applicationUser, new Claim(JwtClaimTypes.Role, role.ToString()));

            Log.Debug($"User {applicationUser.UserName} created");
        }
    }

    private async Task<bool> DoesUserExistAsync(UserManager<ApplicationUser> userManager, ApplicationUser applicationUser)
    {
        var userInDatabase = await userManager.FindByNameAsync(applicationUser.UserName);

        if (userInDatabase != null)
        {
            Log.Debug($"User exists: {applicationUser.UserName}");
            return false;
        }

        return true;
    }

    private async Task CreateUserAsync(UserManager<ApplicationUser> userManager, ApplicationUser applicationUser)
    {
        var createUserResult = await userManager.CreateAsync(applicationUser,
            _appSettings.Value.DefaultUserPassword ?? throw new ArgumentNullException(_appSettings.Value.DefaultUserPassword));

        if (!createUserResult.Succeeded) RaiseIdentityError(createUserResult);
    }

    private async Task AssignToRoleAsync(UserManager<ApplicationUser> userManager, ApplicationUser applicationUser, Roles role)
    {
        var addRolesResult = await userManager.AddToRoleAsync(applicationUser, role.ToString());

        if (!addRolesResult.Succeeded) RaiseIdentityError(addRolesResult);
    }

    private async Task AddClaimsAsync(UserManager<ApplicationUser> userManager, ApplicationUser applicationUser, params Claim[] claims)
    {
        var defaultClaims = new List<Claim>
        {
            new(JwtClaimTypes.Email, applicationUser.Email),
            new(JwtClaimTypes.Id, applicationUser.Id)
        }.Concat(claims);

        var addClaimsResult = await userManager.AddClaimsAsync(applicationUser, defaultClaims);
        if (!addClaimsResult.Succeeded) RaiseIdentityError(addClaimsResult);
    }

    private void RaiseIdentityError(IdentityResult failedResult)
    {
        var message = $"Identity operation failed: {GetErrorsAsString(failedResult)}";
        Log.Error(message);
        throw new Exception(message);
    }

    private string GetErrorsAsString(IdentityResult result)
    {
        return string.Join(Environment.NewLine, result.Errors.Select(x => x.Description));
    }
}