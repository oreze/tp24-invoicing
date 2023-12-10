using System.Text.Json.Serialization;

namespace Identity.Receivables.ApplicationContracts.DTOs;

public class GetInvoiceDTO
{
    [JsonPropertyName("invoiceID")] public int InvoiceID { get; set; }

    [JsonPropertyName("reference")] public string Reference { get; set; }

    [JsonPropertyName("currencyCode")] public string CurrencyCode { get; set; }

    [JsonPropertyName("issueDate")] public string IssueDate { get; set; }

    [JsonPropertyName("openingValue")] public decimal OpeningValue { get; set; }

    [JsonPropertyName("paidValue")] public decimal PaidValue { get; set; }

    [JsonPropertyName("dueDate")] public string DueDate { get; set; }

    [JsonPropertyName("closedDate")] public DateTime? ClosedDate { get; set; } // optional

    [JsonPropertyName("cancelled")] public DateTime? Cancelled { get; set; } // optional

    [JsonPropertyName("debtor")] public DebtorDTO Debtor { get; set; }

    [JsonPropertyName("currency")] public CurrencyDTO Currency { get; set; }
}