using Invoicing.Receivables.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invoicing.Receivables.Infrastructure.Configuration.EntitiesConfiguration;

public class DebtorTypeEntityConfiguration : IEntityTypeConfiguration<Debtor>
{
    public void Configure(EntityTypeBuilder<Debtor> builder)
    {
        builder.HasKey(d => d.ID);

        builder.Property(d => d.Reference).HasMaxLength(255).IsRequired();
        builder.Property(d => d.Name).HasMaxLength(255).IsRequired();
        builder.Property(d => d.Address1).HasMaxLength(255);
        builder.Property(d => d.Address2).HasMaxLength(255);
        builder.Property(d => d.Town).HasMaxLength(255);
        builder.Property(d => d.State).HasMaxLength(255);
        builder.Property(d => d.Zip).HasMaxLength(20);
        builder.Property(d => d.CountryCode).HasMaxLength(2).IsRequired(); // Assuming ISO country code is a 2-character code
        builder.Property(d => d.RegistrationNumber).HasMaxLength(50);

        builder.HasIndex(x => x.Reference).IsUnique();
    }
}