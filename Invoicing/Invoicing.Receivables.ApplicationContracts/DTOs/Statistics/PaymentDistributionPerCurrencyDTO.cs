using System.Text.Json.Serialization;
using Identity.Receivables.ApplicationContracts.DTOs.Enums;

namespace Identity.Receivables.ApplicationContracts.DTOs.Statistics;

public class PaymentDistributionPerCurrencyDTO
{
    [JsonPropertyName("values")] public IDictionary<ApiInvoicePaymentStatus, IEnumerable<MoneyDTO>> Values { get; set; }
}