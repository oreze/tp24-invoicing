using Identity.Receivables.ApplicationContracts.DTOs.Statistics;
using Invoicing.Receivables.Infrastructure.Queries;
using Invoicing.Receivables.Infrastructure.Services;
using MediatR;

namespace Invoicing.Receivables.Infrastructure.Handlers;

public class GetAverageTransactionValuePerCurrencyQueryHandler
    : IRequestHandler<GetAverageTransactionValuePerCurrencyQuery, AverageTransactionValuePerCurrencyDTO>
{
    private readonly IStatisticsService _statisticsService;

    public GetAverageTransactionValuePerCurrencyQueryHandler(IStatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }

    public async Task<AverageTransactionValuePerCurrencyDTO> Handle(GetAverageTransactionValuePerCurrencyQuery request, CancellationToken cancellationToken)
    {
        return await _statisticsService.GetAverageTransactionValuePerCurrencyAsync();
    }
}