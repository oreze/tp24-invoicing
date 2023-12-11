using Identity.Receivables.ApplicationContracts.DTOs.Invoices;
using MediatR;

namespace Invoicing.Receivables.Infrastructure.Commands;

public record CreateInvoiceCommand(CreateInvoiceDTO createInvoiceDTO) : IRequest<int>;