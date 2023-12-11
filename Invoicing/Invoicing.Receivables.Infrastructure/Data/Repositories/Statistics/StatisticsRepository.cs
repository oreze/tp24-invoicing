using Invoicing.Receivables.Domain.Enums;
using Invoicing.Receivables.Domain.ValueObjects;
using Invoicing.Receivables.Domain.ValueObjects.Statistics;
using Microsoft.EntityFrameworkCore;

namespace Invoicing.Receivables.Infrastructure.Data.Repositories.Statistics;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly AppDbContext _dbContext;

    public StatisticsRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<TotalRevenuePerCurrency> GetTotalRevenuePerCurrencyAsync()
    {
        IList<Money> totalRevenuePerCurrency = await _dbContext.Invoices
            .Where(i => i.ClosedDate != null)
            .GroupBy(i => i.CurrencyCode)
            .Select(group =>
                new Money(group.Sum(i => i.OpeningValue), group.Key)
            ).ToListAsync();

        return new TotalRevenuePerCurrency(totalRevenuePerCurrency);
    }

    public async Task<AverageTransactionValuePerCurrency> GetAverageTransactionValuePerCurrencyAsync()
    {
        IEnumerable<Money> averageTransactionValuePerCurrency = await _dbContext.Invoices
            .Where(i => i.ClosedDate != null)
            .GroupBy(i => i.CurrencyCode)
            .Select(group =>
                new Money(group.Average(i => i.PaidValue), group.Key))
            .ToListAsync();

        return new AverageTransactionValuePerCurrency(averageTransactionValuePerCurrency);
    }

    public async Task<PaymentDistributionPerCurrency> GetPaymentStatusDistributionPerCurrencyAsync()
    {
        var invoices = await _dbContext.Invoices
            .Select(i => new
            {
                ///I didn't want to load entire table into app memory, that's how i managed to translate
                /// entire function into SQL. Should be refactored or processed in batch if performance is a concern, otherwise
                /// just load the table and use c# function
                PaymentStatus = i.PaidValue == i.OpeningValue
                    ? InvoicePaymentStatus.Paid
                    : i.ClosedDate != null && i.PaidValue < i.OpeningValue
                        ? InvoicePaymentStatus.Closed
                        : i.DueDate < DateTime.Today
                            ? InvoicePaymentStatus.Overdue
                            : i.Cancelled != null
                                ? InvoicePaymentStatus.Canceled
                                : InvoicePaymentStatus.Awaiting,
                i.CurrencyCode,
                i.PaidValue
            })
            .GroupBy(result => new { result.PaymentStatus, result.CurrencyCode })
            .Select(group =>
                new
                {
                    group.Key.PaymentStatus,
                    group.Key.CurrencyCode,
                    TotalPaidValue = group.Sum(i => i.PaidValue)
                })
            .GroupBy(result => result.PaymentStatus, result => new Money(result.TotalPaidValue, result.CurrencyCode))
            .ToDictionaryAsync(group => group.Key, group => group.AsEnumerable());

        return new PaymentDistributionPerCurrency(invoices);
    }
}