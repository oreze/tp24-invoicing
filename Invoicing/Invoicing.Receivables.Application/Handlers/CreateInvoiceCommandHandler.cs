using Invoicing.Receivables.Infrastructure.Commands;
using Invoicing.Receivables.Infrastructure.Services;
using MediatR;

namespace Invoicing.Receivables.Infrastructure.Handlers;

public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, int>
{
    private readonly IInvoiceService _invoiceService;

    public CreateInvoiceCommandHandler(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    public async Task<int> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        return await _invoiceService.CreateInvoiceAsync(request.createInvoiceDTO);
    }
}