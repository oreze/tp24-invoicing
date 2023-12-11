using Invoicing.Receivables.Domain.Enums;
using Invoicing.Receivables.Domain.Exceptions;

namespace Invoicing.Receivables.Domain.Entities;

public class Invoice
{
    private Invoice()
    {
    }

    public int ID { get; }
    public string Reference { get; private set; }
    public DateTime IssueDate { get; private set; }
    public decimal OpeningValue { get; private set; }
    public decimal PaidValue { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? ClosedDate { get; private set; }
    public DateTime? Cancelled { get; private set; }
    public Debtor Debtor { get; private set; }
    public int DebtorID { get; }
    public Currency Currency { get; private set; }
    public string CurrencyCode { get; }

    public static Invoice Create(string reference, DateTime issueDate, decimal openingValue, decimal paidValue, DateTime dueDate, DateTime? closedDate, DateTime? cancelled, Debtor debtor, Currency currency)
    {
        ValidateInput(reference, issueDate, openingValue, paidValue, dueDate, closedDate, cancelled, debtor, currency);

        return new Invoice
        {
            Reference = reference,
            IssueDate = issueDate,
            OpeningValue = openingValue,
            PaidValue = paidValue,
            DueDate = dueDate,
            ClosedDate = closedDate,
            Cancelled = cancelled,
            Debtor = debtor,
            Currency = currency
        };
    }

    private static void ValidateInput(string reference, DateTime issueDate, decimal openingValue, decimal paidValue,
        DateTime dueDate, DateTime? closedDate, DateTime? cancelled, Debtor debtor, Currency currency)
    {
        if (string.IsNullOrWhiteSpace(reference))
            throw new InputNullException(nameof(reference), "Invoice reference cannot be null or empty.");

        if (issueDate == default)
            throw new InputException(nameof(issueDate), "Invalid issue date for the invoice.");

        if (openingValue < 0)
            throw new InputException(nameof(openingValue), "Opening value must be non-negative.");

        if (paidValue < 0 || paidValue > openingValue)
            throw new InputException(nameof(paidValue),
                "Paid value is invalid. It has to be between 0 and opening value.");

        if (dueDate == default || dueDate < issueDate)
            throw new InputException(nameof(dueDate), "Invalid due date for the invoice.");

        if (closedDate.HasValue && closedDate > DateTime.UtcNow)
            throw new InputException(nameof(closedDate),
                "Invalid closed date for the invoice. The closing date cannot be in the future.");

        if (cancelled.HasValue && cancelled > DateTime.UtcNow)
            throw new InputException(nameof(cancelled),
                "Invalid cancelled date for the invoice. The cancelled date cannot be in the future.");

        if (debtor == default)
            throw new InputException(nameof(debtor), "Invoice debtor cannot be null.");

        if (currency == default)
            throw new InputException(nameof(currency), "Invoice currency cannot be null.");
    }
}
