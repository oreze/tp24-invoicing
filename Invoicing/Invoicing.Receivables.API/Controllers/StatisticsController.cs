using Invoicing.Receivables.Infrastructure.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Invoicing.Receivables.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StatisticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("totalRevenuePerCurrency")]
    public async Task<IActionResult> GetTotalRevenuePerCurrency()
    {
        var query = new GetTotalRevenuePerCurrencyQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("paymentDistributionPerCurrency")]
    public async Task<IActionResult> GetPaymentDistributionPerCurrency()
    {
        var query = new GetPaymentDistributionPerCurrencyQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("averageTransactionValuePerCurrency")]
    public async Task<IActionResult> GetAverageTransactionValuePerCurrency()
    {
        var query = new GetAverageTransactionValuePerCurrencyQuery();
        var result = await _mediator.Send(query);

        return Ok(result);
    }
}
