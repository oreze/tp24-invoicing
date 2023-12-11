using Identity.Receivables.ApplicationContracts.DTOs.Statistics;
using Invoicing.Receivables.Infrastructure.Queries;
using Invoicing.Receivables.Infrastructure.Services;
using MediatR;

namespace Invoicing.Receivables.Infrastructure.Handlers;

public class GetPaymentDistributionPerCurrencyQueryHandler
    : IRequestHandler<GetPaymentDistributionPerCurrencyQuery, PaymentDistributionPerCurrencyDTO>
{
    private readonly IStatisticsService _statisticsService;

    public GetPaymentDistributionPerCurrencyQueryHandler(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    public async Task<PaymentDistributionPerCurrencyDTO> Handle(GetPaymentDistributionPerCurrencyQuery request, CancellationToken cancellationToken)
    {
        return await _statisticsService.GetPaymentStatusDistributionPerCurrencyAsync();
    }
}