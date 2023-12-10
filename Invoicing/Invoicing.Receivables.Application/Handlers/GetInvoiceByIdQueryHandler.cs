using Identity.Receivables.ApplicationContracts.DTOs;
using Invoicing.Receivables.Infrastructure.Queries;
using Invoicing.Receivables.Infrastructure.Services;
using MediatR;

namespace Invoicing.Receivables.Infrastructure.Handlers;

public class GetInvoiceByIdQueryHandler : IRequestHandler<GetInvoiceByIdQuery, GetInvoiceDTO>
{
    private readonly IInvoiceService _invoiceService;

    public GetInvoiceByIdQueryHandler(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    public async Task<GetInvoiceDTO> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
    {
        return await _invoiceService.GetInvoiceByIdAsync(request.id);
    }
}