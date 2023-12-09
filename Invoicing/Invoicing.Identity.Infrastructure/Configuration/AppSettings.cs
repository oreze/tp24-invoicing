namespace Invoicing.Identity.API.Configuration;

public class AppSettings
{
    public string? DefaultAdminUsername { get; set; }
    public string? DefaultAdminEmail { get; set; }
    public string? DefaultCompanyUsername { get; set; }
    public string? DefaultCompanyEmail { get; set; }
    public string? DefaultUserPassword { get; set; }
    public string? DefaultOAuth2ClientID { get; set; }
    public string? DefaultOAuth2ClientSecret { get; set; }
}