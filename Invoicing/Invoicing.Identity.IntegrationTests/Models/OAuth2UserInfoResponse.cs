using System.Text.Json.Serialization;
using Invoicing.Identity.Domain.Enums;

namespace Invoicing.Identity.Tests.Models;

public class OAuth2UserInfoResponse
{
    [JsonPropertyName("sub")] public string Sub { get; set; }

    [JsonPropertyName("email")] public string Email { get; set; }

    [JsonPropertyName("role")] public Roles Role { get; set; }

    [JsonPropertyName("preferred_username")]
    public string PreferredUsername { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("email_verified")] public bool EmailVerified { get; set; }
}