using System.Text.Json.Serialization;

namespace Identity.Receivables.ApplicationContracts.DTOs.Statistics;

public class TotalRevenuePerCurrencyDTO
{
    [JsonPropertyName("values")] public IEnumerable<MoneyDTO> Values { get; set; }
}