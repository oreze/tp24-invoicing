using Identity.Receivables.ApplicationContracts.DTOs;
using MediatR;

namespace Invoicing.Receivables.Infrastructure.Queries;

public record GetInvoiceByIdQuery(int id) : IRequest<GetInvoiceDTO>;