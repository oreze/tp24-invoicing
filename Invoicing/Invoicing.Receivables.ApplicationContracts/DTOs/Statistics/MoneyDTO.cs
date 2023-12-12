using System.Text.Json.Serialization;

namespace Identity.Receivables.ApplicationContracts.DTOs.Statistics;

public class MoneyDTO
{
    [JsonPropertyName("amount")] public decimal Amount { get; set; }

    [JsonPropertyName("currencyCode")] public string CurrencyCode { get; set; }
}