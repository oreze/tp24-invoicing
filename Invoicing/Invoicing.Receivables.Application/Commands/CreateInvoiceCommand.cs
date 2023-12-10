using Identity.Receivables.ApplicationContracts.DTOs;
using MediatR;

namespace Invoicing.Receivables.Infrastructure.Commands;

public record CreateInvoiceCommand(CreateInvoiceDTO createInvoiceDTO) : IRequest<int>;