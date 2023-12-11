namespace Identity.Receivables.ApplicationContracts.DTOs.Enums;

public enum ApiInvoicePaymentStatus
{
    Invalid = 0,
    Awaiting,
    Paid,
    Closed,
    Overdue,
    Canceled
}