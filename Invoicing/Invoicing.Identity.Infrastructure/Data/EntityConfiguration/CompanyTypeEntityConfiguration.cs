using Invoicing.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invoicing.Identity.Infrastructure.Data.EntityConfiguration;

public class CompanyTypeEntityConfiguration: IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder
            .Property(company => company.CompanyName)
            .IsRequired();
        
        builder
            .Property(company => company.GlobalCompanyIdentifier)
            .HasMaxLength(20)
            .IsRequired();
        
        builder
            .HasOne(company => company.ApplicationUser)
            .WithOne(applicationUser => applicationUser.Company);
    }
}
