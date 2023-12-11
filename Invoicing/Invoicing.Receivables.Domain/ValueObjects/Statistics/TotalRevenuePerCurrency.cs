using Invoicing.Receivables.Domain.Exceptions;

namespace Invoicing.Receivables.Domain.ValueObjects.Statistics;

public class TotalRevenuePerCurrency
{
    public TotalRevenuePerCurrency(IEnumerable<Money> totalRevenueInCurrency)
    {
        Validate(totalRevenueInCurrency);

        Values = totalRevenueInCurrency;
    }

    public IEnumerable<Money> Values { get; }

    private void Validate(IEnumerable<Money> totalRevenueInCurrency)
    {
        if (totalRevenueInCurrency == null)
            throw new InputNullException(nameof(totalRevenueInCurrency), "Total revenue per currency list cannot be null.");

        if (totalRevenueInCurrency.Any(i => i == null))
            throw new InputNullException(nameof(totalRevenueInCurrency),
                "Elements of the list cannot be null.");
    }
}
