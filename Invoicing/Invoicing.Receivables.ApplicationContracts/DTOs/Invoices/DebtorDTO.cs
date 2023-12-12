using System.Text.Json.Serialization;

namespace Identity.Receivables.ApplicationContracts.DTOs.Invoices;

public class DebtorDTO
{
    [JsonPropertyName("id")] public int ID { get; set; }

    [JsonPropertyName("reference")] public string Reference { get; set; }

    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("address1")] public string? Address1 { get; set; }

    [JsonPropertyName("address2")] public string? Address2 { get; set; }

    [JsonPropertyName("town")] public string? Town { get; set; }

    [JsonPropertyName("state")] public string? State { get; set; }

    [JsonPropertyName("zip")] public string? Zip { get; set; }

    [JsonPropertyName("countryCode")] public string CountryCode { get; set; }

    [JsonPropertyName("registrationNumber")]
    public string? RegistrationNumber { get; set; }
}