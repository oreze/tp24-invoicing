namespace Invoicing.Identity.Domain.Entities;

public class Company
{
    public int ID { get; set; }
    public string CompanyName { get; set; }
    /// <summary>
    /// LEI identifier, ISO 17442
    /// </summary>
    public string GlobalCompanyIdentifier { get; set; }
    public string? ApplicationUserID { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
}
