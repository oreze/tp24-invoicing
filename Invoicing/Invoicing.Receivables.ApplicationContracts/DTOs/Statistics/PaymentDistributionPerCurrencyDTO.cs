using Identity.Receivables.ApplicationContracts.DTOs.Enums;

namespace Identity.Receivables.ApplicationContracts.DTOs.Statistics;

public class PaymentDistributionPerCurrencyDTO
{
    public IDictionary<ApiInvoicePaymentStatus, IEnumerable<MoneyDTO>> Values { get; set; }
}