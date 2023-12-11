using System.Globalization;
using Invoicing.Receivables.Domain.Exceptions;

namespace Invoicing.Receivables.Domain.ValueObjects;

public class Money
{
    private readonly Lazy<IList<string>> currencyCodes = new(GetCurrencyCodes);

    public Money(decimal amount, string currencyCode)
    {
        ValidateCurrencyCode(currencyCode);

        Amount = amount;
        CurrencyCode = currencyCode;
    }

    public decimal Amount { get; private set; }
    public string CurrencyCode { get; private set; }

    private void ValidateCurrencyCode(string currencyCode)
    {
        if (!GetCurrencyCodes().Contains(currencyCode))
            throw new InputException(nameof(currencyCode), "Currency code is invalid.");
    }

    private static IList<string> GetCurrencyCodes()
    {
        var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(culture => new RegionInfo(culture.Name))
            .Where(IsValidCurrency);

        return regions
            .Select(region => region.ISOCurrencySymbol)
            .ToList();
    }

    private static bool IsValidCurrency(RegionInfo region)
    {
        return !string.IsNullOrWhiteSpace(region.ISOCurrencySymbol);
    }
}