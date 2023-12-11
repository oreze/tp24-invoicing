using System.Text.Json.Serialization;

namespace Identity.Receivables.ApplicationContracts.DTOs.Common;

public class ErrorResponse
{
    [JsonPropertyName("message")] public string Message { get; set; }
}