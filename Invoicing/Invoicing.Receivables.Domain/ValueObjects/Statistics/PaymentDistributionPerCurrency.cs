using Invoicing.Receivables.Domain.Enums;
using Invoicing.Receivables.Domain.Exceptions;

namespace Invoicing.Receivables.Domain.ValueObjects.Statistics;

public class PaymentDistributionPerCurrency
{
    public PaymentDistributionPerCurrency(IDictionary<InvoicePaymentStatus, IEnumerable<Money>> invoicePaymentTypePerCurrency)
    {
        Validate(invoicePaymentTypePerCurrency);

        Values = invoicePaymentTypePerCurrency;
    }

    public IDictionary<InvoicePaymentStatus, IEnumerable<Money>> Values { get; }

    private void Validate(IDictionary<InvoicePaymentStatus, IEnumerable<Money>> invoicePaymentTypePerCurrency)
    {
        if (invoicePaymentTypePerCurrency == null)
            throw new InputNullException(nameof(invoicePaymentTypePerCurrency),
                "Invoice payment type per currency list cannot be null.");

        if (invoicePaymentTypePerCurrency.Values.Any(i => i.Any(i => i == null)))
            throw new InputNullException(nameof(invoicePaymentTypePerCurrency),
                "Elements of the list cannot be null.");
    }
}
