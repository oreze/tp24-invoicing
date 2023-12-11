using Identity.Receivables.ApplicationContracts.DTOs.Statistics;

namespace Invoicing.Receivables.UnitTests.Common.EqualityComparers;

public class PaymentDistributionPerCurrencyDTOEqualityComparer : IEqualityComparer<PaymentDistributionPerCurrencyDTO>
{
    public bool Equals(PaymentDistributionPerCurrencyDTO x, PaymentDistributionPerCurrencyDTO y)
    {
        return x.Values.Count() == y.Values.Count() && x.Values.All(pair => y.Values.TryGetValue(pair.Key, out var values) &&
                                                                            pair.Value.SequenceEqual(values, new MoneyDTOEqualityComparer()));
    }

    public int GetHashCode(PaymentDistributionPerCurrencyDTO obj)
    {
        return obj.Values.GetHashCode();
    }
}