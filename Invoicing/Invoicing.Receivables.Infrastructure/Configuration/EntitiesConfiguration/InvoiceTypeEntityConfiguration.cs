using Invoicing.Receivables.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Invoicing.Receivables.Infrastructure.Configuration.EntitiesConfiguration;

public class InvoiceTypeEntityConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(i => i.ID);

        builder.Property(i => i.Reference).HasMaxLength(512).IsRequired();
        builder.Property(i => i.IssueDate).IsRequired();
        builder.Property(i => i.OpeningValue).IsRequired();
        builder.Property(i => i.PaidValue).IsRequired();
        builder.Property(i => i.DueDate).IsRequired();
        builder.Property(i => i.ClosedDate);
        builder.Property(i => i.Cancelled);

        builder.HasIndex(i => i.Reference).IsUnique();

        builder.HasOne(i => i.Debtor)
            .WithMany()
            .HasForeignKey(i => i.DebtorID);

        builder.HasOne(i => i.Currency)
            .WithMany()
            .HasForeignKey(i => i.CurrencyCode);
    }
}