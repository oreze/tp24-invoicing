using Identity.Receivables.ApplicationContracts.DTOs.Enums;
using Identity.Receivables.ApplicationContracts.DTOs.Statistics;
using Invoicing.Receivables.Domain.Enums;
using Invoicing.Receivables.Domain.ValueObjects;
using Invoicing.Receivables.Domain.ValueObjects.Statistics;
using Invoicing.Receivables.Infrastructure.Data.Repositories.Statistics;

namespace Invoicing.Receivables.Infrastructure.Services;

public class StatisticsService : IStatisticsService
{
    private readonly IStatisticsRepository _statisticsRepository;

    public StatisticsService(IStatisticsRepository statisticsRepository)
    {
        _statisticsRepository = statisticsRepository;
    }

    public async Task<TotalRevenuePerCurrencyDTO> GetTotalRevenuePerCurrencyAsync()
    {
        var result = await _statisticsRepository.GetTotalRevenuePerCurrencyAsync();

        return new TotalRevenuePerCurrencyDTO
        {
            Values = result.Values.Select(item => new MoneyDTO
            {
                CurrencyCode = item.CurrencyCode,
                Amount = item.Amount
            })
        };
    }

    public async Task<AverageTransactionValuePerCurrencyDTO> GetAverageTransactionValuePerCurrencyAsync()
    {
        var result = await _statisticsRepository.GetAverageTransactionValuePerCurrencyAsync();

        return new AverageTransactionValuePerCurrencyDTO
        {
            Values = result.Values.Select(MapToMoneyDTO)
        };
    }

    public async Task<PaymentDistributionPerCurrencyDTO> GetPaymentStatusDistributionPerCurrencyAsync()
    {
        var result = await _statisticsRepository.GetPaymentStatusDistributionPerCurrencyAsync();

        return MapToDTO(result);
    }

    private PaymentDistributionPerCurrencyDTO MapToDTO(PaymentDistributionPerCurrency paymentDistributionPerCurrency)
    {
        var mappedDictionary = new Dictionary<ApiInvoicePaymentStatus, IEnumerable<MoneyDTO>>();

        foreach (var kvp in paymentDistributionPerCurrency.Values)
        {
            var apiStatus = MapToApiInvoicePaymentStatus(kvp.Key);
            var moneyDTOs = kvp.Value.Select(MapToMoneyDTO);

            mappedDictionary.Add(apiStatus, moneyDTOs);
        }

        return new PaymentDistributionPerCurrencyDTO { Values = mappedDictionary };
    }

    private static ApiInvoicePaymentStatus MapToApiInvoicePaymentStatus(InvoicePaymentStatus status)
    {
        return status switch
        {
            InvoicePaymentStatus.Awaiting => ApiInvoicePaymentStatus.Awaiting,
            InvoicePaymentStatus.Paid => ApiInvoicePaymentStatus.Paid,
            InvoicePaymentStatus.Closed => ApiInvoicePaymentStatus.Closed,
            InvoicePaymentStatus.Overdue => ApiInvoicePaymentStatus.Overdue,
            InvoicePaymentStatus.Canceled => ApiInvoicePaymentStatus.Canceled,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }

    private MoneyDTO MapToMoneyDTO(Money money)
    {
        return new MoneyDTO
        {
            CurrencyCode = money.CurrencyCode,
            Amount = money.Amount
        };
    }
}