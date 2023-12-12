using Identity.Receivables.ApplicationContracts.DTOs.Statistics;

namespace Invoicing.Receivables.UnitTests.Common.EqualityComparers;

public class MoneyDTOEqualityComparer : IEqualityComparer<MoneyDTO>
{
    public bool Equals(MoneyDTO x, MoneyDTO y)
    {
        return x.CurrencyCode == y.CurrencyCode && x.Amount == y.Amount;
    }

    public int GetHashCode(MoneyDTO obj)
    {
        return HashCode.Combine(obj.CurrencyCode, obj.Amount);
    }
}