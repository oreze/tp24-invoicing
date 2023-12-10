using Microsoft.EntityFrameworkCore;

namespace Invoicing.Receivables.Infrastructure.Data.Repositories.Invoice;

public class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _dbContext;

    public InvoiceRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<Domain.Entities.Invoice> GetByIdAsync(int id)
    {
        return await _dbContext.Invoices
            .Include(i => i.Debtor)
            .Include(i => i.Currency)
            .FirstOrDefaultAsync(i => i.ID == id);
    }

    public async Task AddAsync(Domain.Entities.Invoice invoice)
    {
        var doesInvoiceExists = await _dbContext.Invoices.AnyAsync(i => i.ID == invoice.ID);

        if (!doesInvoiceExists) await _dbContext.Invoices.AddAsync(invoice);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}