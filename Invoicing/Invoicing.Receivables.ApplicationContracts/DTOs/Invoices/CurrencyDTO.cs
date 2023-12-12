using System.Text.Json.Serialization;

namespace Identity.Receivables.ApplicationContracts.DTOs.Invoices;

public class CurrencyDTO
{
    [JsonPropertyName("code")] public string Code { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }
}