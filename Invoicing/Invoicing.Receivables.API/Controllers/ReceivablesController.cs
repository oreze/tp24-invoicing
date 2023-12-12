using Identity.Receivables.ApplicationContracts.DTOs.Invoices;
using Invoicing.Receivables.Infrastructure.Commands;
using Invoicing.Receivables.Infrastructure.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Invoicing.Receivables.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ReceivablesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReceivablesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInvoiceById(int id)
    {
        var query = new GetInvoiceByIdQuery(id);
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDTO dto)
    {
        var command = new CreateInvoiceCommand(dto);
        var newInvoiceId = await _mediator.Send(command);

        return Created($"/api/invoices/{newInvoiceId}", newInvoiceId);
    }
}