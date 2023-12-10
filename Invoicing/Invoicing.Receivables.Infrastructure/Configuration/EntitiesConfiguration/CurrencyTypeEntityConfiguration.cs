using Invoicing.Receivables.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invoicing.Receivables.Infrastructure.Configuration.EntitiesConfiguration;

public class CurrencyTypeEntityConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.HasKey(c => c.Code);

        builder.Property(c => c.Code).HasMaxLength(3); // ISO 4217
        builder.Property(d => d.Name).HasMaxLength(128).IsRequired();
    }
}