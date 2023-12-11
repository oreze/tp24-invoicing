using Invoicing.Receivables.Domain.Enums;
using Invoicing.Receivables.Domain.Exceptions;
using Invoicing.Receivables.Domain.ValueObjects;
using Invoicing.Receivables.Domain.ValueObjects.Statistics;

namespace Invoicing.Receivables.UnitTests.Domain.ValueObjects.Statistics;

public class PaymentDistributionPerCurrencyTests
{
    [Fact]
    public void PaymentDistributionPerCurrency_Constructor_ValidValues_CreatesInstance()
    {
        // Arrange
        var paymentTypePerCurrency = new Dictionary<InvoicePaymentStatus, IEnumerable<Money>>
        {
            { InvoicePaymentStatus.Paid, new List<Money> { new(100.0m, "USD"), new(50.0m, "EUR") } },
            { InvoicePaymentStatus.Awaiting, new List<Money> { new(75.0m, "GBP") } }
        };

        // Act
        var paymentDistributionPerCurrency = new PaymentDistributionPerCurrency(paymentTypePerCurrency);

        // Assert
        Assert.Equal(paymentTypePerCurrency, paymentDistributionPerCurrency.Values);
    }

    [Fact]
    public void PaymentDistributionPerCurrency_Constructor_NullValues_ThrowsInputNullException()
    {
        // Act and Assert
        Assert.Throws<InputNullException>(() => new PaymentDistributionPerCurrency(null));
    }

    [Fact]
    public void Constructor_NullItemInList_ThrowsInputNullException()
    {
        var paymentTypePerCurrency = new Dictionary<InvoicePaymentStatus, IEnumerable<Money>>
        {
            { InvoicePaymentStatus.Paid, new List<Money> { new(100.0m, "USD"), new(50.0m, "EUR") } },
            { InvoicePaymentStatus.Awaiting, new List<Money> { new(75.0m, "GBP"), null } }
        };

        // Act and Assert
        Assert.Throws<InputNullException>(() => new PaymentDistributionPerCurrency(paymentTypePerCurrency));
    }
}