namespace Invoicing.Receivables.Infrastructure.Data.Repositories.Invoice;

public interface IInvoiceRepository
{
    Task<Domain.Entities.Invoice> GetByIdAsync(int id);
    Task AddAsync(Domain.Entities.Invoice invoice);
    Task<int> SaveChangesAsync();
}