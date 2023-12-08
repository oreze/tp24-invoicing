using Microsoft.AspNetCore.Identity;

namespace Invoicing.Identity.Domain.Entities;

public class ApplicationUser: IdentityUser
{
    public int? CompanyID { get; set; }
    public Company? Company { get; set; }
}
