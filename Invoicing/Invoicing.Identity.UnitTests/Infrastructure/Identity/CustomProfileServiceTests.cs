using System.Security.Claims;
using Bogus;
using Duende.IdentityServer.Models;
using Invoicing.Identity.Domain.Entities;
using Invoicing.Identity.Domain.Enums;
using Invoicing.Identity.Infrastructure.Configuration.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Invoicing.Identity.UnitTests.Infrastructure.Identity;

public class CustomProfileServiceTests
{
    [Fact]
    public async Task GetProfileDataAsync_UserExists_GetUserClaims()
    {
        //arrange
        var userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        var roles = Enum.GetNames<Roles>();
        userManagerMock
            .Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(GetFakeUser());
        userManagerMock
            .Setup(manager => manager.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(Enum.GetNames<Roles>());

        var context = new ProfileDataRequestContext
        {
            Subject = new ClaimsPrincipal(Enumerable.Empty<ClaimsIdentity>())
        };

        var userManager = userManagerMock.Object;
        var customProfileService = new CustomProfileService(userManager);

        //act
        await customProfileService.GetProfileDataAsync(context);

        //assert
        foreach (var expectedRole in roles) Assert.Contains(context.IssuedClaims, claim => claim.Type == ClaimTypes.Role && claim.Value == expectedRole);

        Assert.Contains(context.IssuedClaims, claim => claim.Type == ClaimTypes.Email);
        Assert.Contains(context.IssuedClaims, claim => claim.Type == ClaimTypes.Name);
    }

    [Fact]
    public async Task GetProfileDataAsync_UserNotExists_ClaimsAreEmpty()
    {
        //arrange
        var userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        userManagerMock
            .Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync((ApplicationUser?)null);

        var context = new ProfileDataRequestContext
        {
            Subject = new ClaimsPrincipal(Enumerable.Empty<ClaimsIdentity>())
        };

        var userManager = userManagerMock.Object;
        var customProfileService = new CustomProfileService(userManager);

        //act
        await customProfileService.GetProfileDataAsync(context);

        //assert
        Assert.Empty(context.IssuedClaims);
    }

    [Fact]
    public async Task IsActiveAsync_UserExists_SetIsActiveToTrue()
    {
        //arrange
        var userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        userManagerMock
            .Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(GetFakeUser);

        var isActiveContext = new IsActiveContext(new ClaimsPrincipal(), new Client(), "randomCaller");
        var userManager = userManagerMock.Object;
        var customProfileService = new CustomProfileService(userManager);

        //act
        await customProfileService.IsActiveAsync(isActiveContext);

        //assert
        Assert.True(isActiveContext.IsActive);
    }

    [Fact]
    public async Task IsActiveAsync_UserNotExists_SetIsActiveToFalse()
    {
        //arrange
        var userManagerMock = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
        userManagerMock
            .Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync((ApplicationUser?)null);

        var isActiveContext = new IsActiveContext(new ClaimsPrincipal(), new Client(), "randomCaller");
        var userManager = userManagerMock.Object;
        var customProfileService = new CustomProfileService(userManager);

        //act
        await customProfileService.IsActiveAsync(isActiveContext);

        //assert
        Assert.False(isActiveContext.IsActive);
    }

    private ApplicationUser GetFakeUser()
    {
        return new Faker<ApplicationUser>()
            .RuleFor(u => u.UserName, f => f.Person.UserName)
            .RuleFor(u => u.Email, f => f.Person.Email)
            .Generate();
    }
}