using Invoicing.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invoicing.Identity.Infrastructure.Data.EntityConfiguration;

public class ApplicationUserTypeEntityConfiguration: IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder
            .HasOne(user => user.Company)
            .WithOne(company => company.ApplicationUser)
            .HasForeignKey<Company>(company => company.ApplicationUserID)
            .IsRequired(false);
    }
}
