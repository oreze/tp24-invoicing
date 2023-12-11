using Invoicing.Receivables.Domain.Entities;
using Invoicing.Receivables.Domain.Exceptions;

namespace Invoicing.Receivables.UnitTests.Domain.Entities;

public class InvoiceTests
{
    public static IEnumerable<object[]> InvalidInputData =>
        new List<object[]>
        {
            new object[] { null, DateTime.Now, 100.00, 50.00, 30, null, null, DateTime.Today.AddDays(1) },
            new object[] { "123", default, 100.00, 50.00, 30, null, null, DateTime.Today.AddDays(1) },
            new object[] { "123", DateTime.Now, -10.00, 50.00, 30, null, null, DateTime.Today.AddDays(1) },
            new object[] { "123", DateTime.Now, 100.00, 120.00, 30, null, null, DateTime.Today.AddDays(1) },
            new object[] { "123", DateTime.Now, 100.00, 120.00, 30, null, null, DateTime.Today.AddDays(1) },
            new object[] { "123", DateTime.Now, 100.00, 120.00, 30, null, null, DateTime.Today.AddDays(-1) },
            new object[] { "123", DateTime.Now, 100.00, 120.00, 30, null, null, default },
            new object[] { "123", DateTime.Now, 100.00, 120.00, 30, null, null, DateTime.Today.AddDays(1) }
        };

    [Fact]
    public void Create_ValidInput_ReturnsInvoiceInstance()
    {
        // Arrange
        var reference = "123";
        var issueDate = DateTime.Now;
        var openingValue = 100.00m;
        var paidValue = 50.00m;
        var dueDate = issueDate.AddDays(30);
        DateTime? closedDate = null;
        DateTime? cancelled = null;
        var debtor = Debtor.Create("DebtorRef", "DebtorName", null, null, null, null, null, "US", null);
        var currency = Currency.Create("USD", "US Dollar");

        // Act
        var invoice = Invoice.Create(reference, issueDate, openingValue, paidValue, dueDate, closedDate, cancelled, debtor, currency);

        // Assert
        Assert.NotNull(invoice);
        Assert.Equal(reference, invoice.Reference);
        Assert.Equal(issueDate, invoice.IssueDate);
        Assert.Equal(openingValue, invoice.OpeningValue);
        Assert.Equal(paidValue, invoice.PaidValue);
        Assert.Equal(dueDate, invoice.DueDate);
        Assert.Null(invoice.ClosedDate);
        Assert.Null(invoice.Cancelled);
        Assert.Equal(debtor, invoice.Debtor);
        Assert.Equal(currency, invoice.Currency);
    }

    [Theory]
    [MemberData(nameof(InvalidInputData))]
    public void Create_InvalidInput_ThrowsException(
        string reference, DateTime issueDate, decimal openingValue, decimal paidValue,
        int dueDateOffsetDays, DateTime? closedDate, DateTime? cancelled, DateTime dueDate)
    {
        // Arrange
        var debtor = Debtor.Create("randomDebtor", "DebtorName", null, null, null, null, null, "US", null);
        var currency = Currency.Create("USD", "US Dollar");

        // Act & Assert
        Assert.ThrowsAny<InputException>(() => Invoice.Create(reference, issueDate, openingValue, paidValue, dueDate, closedDate, cancelled, debtor, currency));
    }


    [Fact]
    public void Create_InvoiceWithNullOrEmptyReference_ThrowsInputNullException()
    {
        var reference = string.Empty;
        var issueDate = DateTime.Now;
        var openingValue = 100.0m;
        var paidValue = 50.0m;
        var dueDate = DateTime.Now.AddDays(30);
        DateTime? closedDate = null;
        DateTime? cancelled = null;
        var debtor = GetDefaultDebtor();
        var currency = GetDefaultCurrency();

        Assert.Throws<InputNullException>(() => { Invoice.Create(reference, issueDate, openingValue, paidValue, dueDate, closedDate, cancelled, debtor, currency); });
    }

    [Fact]
    public void Create_InvoiceWithDefaultIssueDate_ThrowsInputException()
    {
        var reference = "ABC123";
        DateTime issueDate = default;
        var openingValue = 100.0m;
        var paidValue = 50.0m;
        var dueDate = DateTime.Now.AddDays(30);
        DateTime? closedDate = null;
        DateTime? cancelled = null;
        var debtor = GetDefaultDebtor();
        var currency = GetDefaultCurrency();

        // Act and Assert
        Assert.Throws<InputException>(() => { Invoice.Create(reference, issueDate, openingValue, paidValue, dueDate, closedDate, cancelled, debtor, currency); });
    }

    [Fact]
    public void Create_InvoiceWithNegativeOpeningValue_ThrowsInputException()
    {
        // Arrange
        var reference = "ABC123";
        var issueDate = DateTime.Now;
        var openingValue = -10.0m;
        var paidValue = 5.0m;
        var dueDate = DateTime.Now.AddDays(30);
        DateTime? closedDate = null;
        DateTime? cancelled = null;
        var debtor = GetDefaultDebtor();
        var currency = GetDefaultCurrency();

        // Act and Assert
        Assert.Throws<InputException>(() => { Invoice.Create(reference, issueDate, openingValue, paidValue, dueDate, closedDate, cancelled, debtor, currency); });
    }

    [Fact]
    public void Create_InvoiceWithInvalidPaidValue_ThrowsInputException()
    {
        // Arrange
        var reference = "ABC123";
        var issueDate = DateTime.Now;
        var openingValue = 100.0m;
        var paidValue = 150.0m;
        var dueDate = DateTime.Now.AddDays(30);
        DateTime? closedDate = null;
        DateTime? cancelled = null;
        var debtor = GetDefaultDebtor();
        var currency = GetDefaultCurrency();

        // Act and Assert
        Assert.Throws<InputException>(() => { Invoice.Create(reference, issueDate, openingValue, paidValue, dueDate, closedDate, cancelled, debtor, currency); });
    }

    [Fact]
    public void Create_InvoiceWithInvalidDueDate_ThrowsInputException()
    {
        // Arrange
        var reference = "ABC123";
        var issueDate = DateTime.Now;
        var openingValue = 100.0m;
        var paidValue = 50.0m;
        DateTime dueDate = default; // or DateTime.MinValue
        DateTime? closedDate = null;
        DateTime? cancelled = null;
        var debtor = GetDefaultDebtor();
        var currency = GetDefaultCurrency();

        // Act and Assert
        Assert.Throws<InputException>(() => { Invoice.Create(reference, issueDate, openingValue, paidValue, dueDate, closedDate, cancelled, debtor, currency); });
    }

    [Fact]
    public void Create_InvoiceWithClosedDateInFuture_ThrowsInputException()
    {
        // Arrange
        var reference = "ABC123";
        var issueDate = DateTime.Now;
        var openingValue = 100.0m;
        var paidValue = 50.0m;
        var dueDate = DateTime.Now.AddDays(30);
        var closedDate = DateTime.UtcNow.AddDays(1);
        DateTime? cancelled = null;
        var debtor = GetDefaultDebtor();
        var currency = GetDefaultCurrency();

        // Act and Assert
        Assert.Throws<InputException>(() => { Invoice.Create(reference, issueDate, openingValue, paidValue, dueDate, closedDate, cancelled, debtor, currency); });
    }

    [Fact]
    public void Create_InvoiceWithCancelledDateInFuture_ThrowsInputException()
    {
        // Arrange
        var reference = "ABC123";
        var issueDate = DateTime.Now;
        var openingValue = 100.0m;
        var paidValue = 50.0m;
        var dueDate = DateTime.Now.AddDays(30);
        DateTime? closedDate = null;
        var cancelledDate = DateTime.UtcNow.AddDays(1);
        var debtor = GetDefaultDebtor();
        var currency = GetDefaultCurrency();

        // Act and Assert
        Assert.Throws<InputException>(() => { Invoice.Create(reference, issueDate, openingValue, paidValue, dueDate, closedDate, cancelledDate, debtor, currency); });
    }

    [Fact]
    public void Create_InvoiceWithClosedAndCancelledDatesInFuture_ThrowsInputException()
    {
        // Arrange
        var reference = "ABC123";
        var issueDate = DateTime.Now;
        var openingValue = 100.0m;
        var paidValue = 50.0m;
        var dueDate = DateTime.Now.AddDays(30);
        var closedDate = DateTime.UtcNow.AddDays(1);
        var cancelledDate = DateTime.UtcNow.AddDays(1);
        var debtor = GetDefaultDebtor();
        var currency = GetDefaultCurrency();

        // Act and Assert
        Assert.Throws<InputException>(() => { Invoice.Create(reference, issueDate, openingValue, paidValue, dueDate, closedDate, cancelledDate, debtor, currency); });
    }

    [Fact]
    public void Create_InvoiceWithNullDebtor_ThrowsInputException()
    {
        // Arrange
        var reference = "ABC123";
        var issueDate = DateTime.Now;
        var openingValue = 100.0m;
        var paidValue = 50.0m;
        var dueDate = DateTime.Now.AddDays(30);
        DateTime? closedDate = null;
        DateTime? cancelled = null;
        Debtor debtor = null;
        var currency = GetDefaultCurrency();

        // Act and Assert
        Assert.Throws<InputException>(() => { Invoice.Create(reference, issueDate, openingValue, paidValue, dueDate, closedDate, cancelled, debtor, currency); });
    }

    [Fact]
    public void Create_InvoiceWithNullCurrency_ThrowsInputException()
    {
        // Arrange
        var reference = "ABC123";
        var issueDate = DateTime.Now;
        var openingValue = 100.0m;
        var paidValue = 50.0m;
        var dueDate = DateTime.Now.AddDays(30);
        DateTime? closedDate = null;
        DateTime? cancelled = null;
        var debtor = GetDefaultDebtor();
        Currency currency = null;

        // Act and Assert
        Assert.Throws<InputException>(() => { Invoice.Create(reference, issueDate, openingValue, paidValue, dueDate, closedDate, cancelled, debtor, currency); });
    }

    private Debtor GetDefaultDebtor()
    {
        return Debtor.Create("randomDebtor", "DebtorName", null, null, null, null, null, "US", null);
    }

    private Currency GetDefaultCurrency()
    {
        return Currency.Create("USD", "US Dollar");
    }
}
