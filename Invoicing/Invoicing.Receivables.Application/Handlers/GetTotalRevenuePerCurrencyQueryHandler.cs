using Identity.Receivables.ApplicationContracts.DTOs.Statistics;
using Invoicing.Receivables.Infrastructure.Queries;
using Invoicing.Receivables.Infrastructure.Services;
using MediatR;

namespace Invoicing.Receivables.Infrastructure.Handlers;

public class GetTotalRevenuePerCurrencyQueryHandler
    : IRequestHandler<GetTotalRevenuePerCurrencyQuery, TotalRevenuePerCurrencyDTO>
{
    private readonly IStatisticsService _statisticsService;

    public GetTotalRevenuePerCurrencyQueryHandler(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    public async Task<TotalRevenuePerCurrencyDTO> Handle(GetTotalRevenuePerCurrencyQuery request, CancellationToken cancellationToken)
    {
        return await _statisticsService.GetTotalRevenuePerCurrencyAsync();
    }
}