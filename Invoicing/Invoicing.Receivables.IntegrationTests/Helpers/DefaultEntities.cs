using Identity.Receivables.ApplicationContracts.DTOs.Invoices;

namespace Invoicing.Receivables.IntegrationTests.Helpers;

public static class DefaultEntities
{
    public static CreateInvoiceDTO GetDefaultCreateInvoiceDTO()
    {
        return new CreateInvoiceDTO
        {
            Reference = Guid.NewGuid().ToString(),
            CurrencyCode = "USD",
            IssueDate = DateTime.Now,
            OpeningValue = 1000.00m,
            PaidValue = 500.00m,
            DueDate = DateTime.Now.AddDays(30),
            ClosedDate = null,
            Cancelled = null,
            DebtorName = "John Doe",
            DebtorReference = "DEB-456",
            DebtorAddress1 = "123 Main St",
            DebtorTown = "Cityville",
            DebtorCountryCode = "US"
        };
    }
}
