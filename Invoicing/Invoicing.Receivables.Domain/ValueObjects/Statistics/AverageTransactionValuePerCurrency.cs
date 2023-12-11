using Invoicing.Receivables.Domain.Exceptions;

namespace Invoicing.Receivables.Domain.ValueObjects.Statistics;

public class AverageTransactionValuePerCurrency
{
    public AverageTransactionValuePerCurrency(IEnumerable<Money> averageTransactionValuesPerCurrency)
    {
        Validate(averageTransactionValuesPerCurrency);

        Values = averageTransactionValuesPerCurrency;
    }

    public IEnumerable<Money> Values { get; }

    private void Validate(IEnumerable<Money> averageTransactionValuePerCurrency)
    {
        if (averageTransactionValuePerCurrency == null)
            throw new InputNullException(nameof(averageTransactionValuePerCurrency),
                "Average transaction value per currency list cannot be null.");

        if (averageTransactionValuePerCurrency.Any(i => i == null))
            throw new InputNullException(nameof(averageTransactionValuePerCurrency),
                "Elements of the list cannot be null.");
    }
}
