namespace Invoicing.Receivables.Domain.Enums;

public enum InvoicePaymentStatus
{
    Invalid = 0,
    Awaiting,
    Paid,
    Closed,
    Overdue,
    Canceled
}