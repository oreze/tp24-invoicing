using Identity.Receivables.ApplicationContracts.DTOs.Statistics;

namespace Invoicing.Receivables.Infrastructure.Services;

public interface IStatisticsService
{
    public Task<TotalRevenuePerCurrencyDTO> GetTotalRevenuePerCurrencyAsync();
    public Task<AverageTransactionValuePerCurrencyDTO> GetAverageTransactionValuePerCurrencyAsync();
    public Task<PaymentDistributionPerCurrencyDTO> GetPaymentStatusDistributionPerCurrencyAsync();
}