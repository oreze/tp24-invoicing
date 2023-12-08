using Invoicing.Identity.Domain.Entities;
using Invoicing.Identity.Domain.Enums;

namespace Invoicing.Identity.API.Seeders;

public static class DefaultEntities
{
    public static readonly IEnumerable<(Roles role, IEnumerable<ApplicationUser>)> DefaultUsers =
        new List<(Roles role, IEnumerable<ApplicationUser>)>
    {
        (
            Roles.Administrator,
            new List<ApplicationUser>()
            {
                new()
                {
                    UserName = "admin",
                    Email = "administrator@email.com",
                    EmailConfirmed = true
                }
            }
        ),
        (
            Roles.Company,
            new List<ApplicationUser>()
            {
                new()
                {
                    Company = new Company
                    {
                        CompanyName = "Random Company #1",
                        GlobalCompanyIdentifier = "12345678912345678912"
                    },
                    UserName = "company-1",
                    Email = "randomcompany@email.com",
                    EmailConfirmed = true
                },
                new()
                {
                    Company = new Company
                    {
                        CompanyName = "Unique Company Name #2",
                        GlobalCompanyIdentifier = "qwertyuioopasdfghklh"
                    },
                    UserName = "company-2",
                    Email = "uniquecompany@email.com",
                    EmailConfirmed = true
                }
            }
        )
    };
}
