using Identity.Receivables.ApplicationContracts.DTOs.Invoices;

namespace Invoicing.Receivables.Infrastructure.Services;

public interface IInvoiceService
{
    Task<GetInvoiceDTO> GetInvoiceByIdAsync(int invoiceId);
    Task<int> CreateInvoiceAsync(CreateInvoiceDTO createInvoiceDto);
}