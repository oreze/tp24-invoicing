using System.Net;
using System.Net.Http.Headers;
using Invoicing.Identity.API.Configuration;
using Invoicing.Identity.Domain.Enums;
using Invoicing.Identity.Tests.Helpers;
using Invoicing.Identity.Tests.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Invoicing.Identity.Tests.Features;

public class TokenTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly IOptions<AppSettings> _appSettings;
    private readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _webApplicationFactory;

    public TokenTests(WebApplicationFactory<Program> webApplicationFactory)
    {
        _webApplicationFactory = webApplicationFactory;
        _httpClient = _webApplicationFactory.CreateDefaultClient();

        using var scope = _webApplicationFactory.Services.CreateScope();
        _appSettings = scope.ServiceProvider.GetService<IOptions<AppSettings>>()
                       ?? throw new ArgumentNullException(nameof(IOptions<AppSettings>));
    }

    [Fact]
    public async Task Token_CanLoginAsDefaultAdmin()
    {
        var response = await _httpClient.PostAsync("/connect/token",
            new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", _appSettings.Value.DefaultOAuth2ClientID),
                new KeyValuePair<string, string>("client_secret", _appSettings.Value.DefaultOAuth2ClientSecret),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", _appSettings.Value.DefaultAdminUsername),
                new KeyValuePair<string, string>("password", _appSettings.Value.DefaultUserPassword)
            }));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var token = await HttpClientHelper.GetDeserializedResponseAsync<OAuth2TokenResponse>(response);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
        var userInfoResponse = await _httpClient.GetAsync("/connect/userinfo");
        Assert.Equal(HttpStatusCode.OK, userInfoResponse.StatusCode);

        var userInfo = await HttpClientHelper.GetDeserializedResponseAsync<OAuth2UserInfoResponse>(userInfoResponse);

        Assert.Equal(Roles.Administrator, userInfo.Role);
        Assert.Equal(_appSettings.Value.DefaultAdminEmail, userInfo.Email);
        Assert.Equal(_appSettings.Value.DefaultAdminUsername, userInfo.Name);
    }

    [Fact]
    public async Task Token_ReturnsBadRequestOnInvalidSecret()
    {
        var response = await _httpClient.PostAsync("/connect/token",
            new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", _appSettings.Value.DefaultOAuth2ClientID),
                new KeyValuePair<string, string>("client_secret", "invalid_secret"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", _appSettings.Value.DefaultAdminUsername),
                new KeyValuePair<string, string>("password", _appSettings.Value.DefaultUserPassword)
            }));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}