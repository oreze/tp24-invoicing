using Invoicing.Identity.Domain.Entities;
using Invoicing.Identity.Infrastructure.Data.EntityConfiguration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Invoicing.Identity.Infrastructure.Data;

public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
{
    public DbSet<Company> Companies { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ApplicationUserTypeEntityConfiguration());
        builder.ApplyConfiguration(new CompanyTypeEntityConfiguration());

        base.OnModelCreating(builder);
    }
}
