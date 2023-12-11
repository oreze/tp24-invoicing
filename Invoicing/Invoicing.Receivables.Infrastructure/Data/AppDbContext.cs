using Invoicing.Receivables.Domain.Entities;
using Invoicing.Receivables.Infrastructure.Configuration.EntitiesConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Invoicing.Receivables.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Invoice> Invoices { get; set; }
    public virtual DbSet<Debtor> Debtors { get; set; }
    public virtual DbSet<Currency> Currencies { get; set; }

    public async Task<bool> IsAnyEntityInDb()
    {
        return await Invoices.AnyAsync() || await Debtors.AnyAsync() || await Currencies.AnyAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InvoiceTypeEntityConfiguration());
        modelBuilder.ApplyConfiguration(new DebtorTypeEntityConfiguration());
        modelBuilder.ApplyConfiguration(new CurrencyTypeEntityConfiguration());
    }
}
