using System.Text.Json.Serialization;

namespace Identity.Receivables.ApplicationContracts.DTOs;

public class ErrorResponse
{
    [JsonPropertyName("message")] public string Message { get; set; }
}