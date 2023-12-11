using Invoicing.Receivables.Domain.ValueObjects.Statistics;

namespace Invoicing.Receivables.Infrastructure.Data.Repositories.Statistics;

public interface IStatisticsRepository
{
    public Task<TotalRevenuePerCurrency> GetTotalRevenuePerCurrencyAsync();
    public Task<AverageTransactionValuePerCurrency> GetAverageTransactionValuePerCurrencyAsync();
    public Task<PaymentDistributionPerCurrency> GetPaymentStatusDistributionPerCurrencyAsync();
}