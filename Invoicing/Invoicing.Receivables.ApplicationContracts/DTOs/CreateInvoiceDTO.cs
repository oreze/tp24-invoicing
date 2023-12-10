using System.Text.Json.Serialization;

namespace Identity.Receivables.ApplicationContracts.DTOs;

public class CreateInvoiceDTO
{
    [JsonPropertyName("reference")] public string Reference { get; set; }

    [JsonPropertyName("currencyCode")] public string CurrencyCode { get; set; }

    [JsonPropertyName("issueDate")] public DateTime IssueDate { get; set; }

    [JsonPropertyName("openingValue")] public decimal OpeningValue { get; set; }

    [JsonPropertyName("paidValue")] public decimal PaidValue { get; set; }

    [JsonPropertyName("dueDate")] public DateTime DueDate { get; set; }

    [JsonPropertyName("closedDate")] public DateTime? ClosedDate { get; set; } // optional

    [JsonPropertyName("cancelled")] public DateTime? Cancelled { get; set; } // optional

    [JsonPropertyName("debtorName")] public string DebtorName { get; set; }

    [JsonPropertyName("debtorReference")] public string DebtorReference { get; set; }

    [JsonPropertyName("debtorAddress1")] public string? DebtorAddress1 { get; set; } // optional

    [JsonPropertyName("debtorAddress2")] public string? DebtorAddress2 { get; set; } // optional

    [JsonPropertyName("debtorTown")] public string? DebtorTown { get; set; } // optional

    [JsonPropertyName("debtorState")] public string? DebtorState { get; set; } // optional

    [JsonPropertyName("debtorZip")] public string? DebtorZip { get; set; } // optional

    [JsonPropertyName("debtorCountryCode")]
    public string DebtorCountryCode { get; set; }

    [JsonPropertyName("debtorRegistrationNumber")]
    public string? DebtorRegistrationNumber { get; set; } // optional
}