using Identity.Receivables.ApplicationContracts.DTOs.Invoices;

namespace Invoicing.Receivables.IntegrationTests.Helpers;

public class CreateInvoiceDTOFactory
{
    private readonly IList<CreateInvoiceDTO> InvoiceDtos;

    public CreateInvoiceDTOFactory()
    {
        InvoiceDtos = new List<CreateInvoiceDTO>();
    }

    public CreateInvoiceDTOFactory CreateAwaitingInvoice(string currencyCode = "EUR", decimal amount = 1500)
    {
        InvoiceDtos.Add(new CreateInvoiceDTO
        {
            Reference = Guid.NewGuid().ToString(),
            CurrencyCode = currencyCode,
            IssueDate = DateTime.Now.AddDays(-15),
            OpeningValue = 1500.0m + amount,
            PaidValue = amount,
            DueDate = DateTime.Now.AddDays(45),
            DebtorName = "Jane Doe",
            DebtorReference = "DEB456",
            DebtorCountryCode = "DE"
        });

        return this;
    }

    public CreateInvoiceDTOFactory CreatePaidInvoice(string currencyCode = "EUR", decimal amount = 1500)
    {
        InvoiceDtos.Add(new CreateInvoiceDTO
        {
            Reference = Guid.NewGuid().ToString(),
            CurrencyCode = currencyCode,
            IssueDate = DateTime.Now.AddDays(-15),
            OpeningValue = amount,
            PaidValue = amount,
            DueDate = DateTime.Now.AddDays(45),
            DebtorName = "Jane Doe",
            DebtorReference = "DEB456",
            DebtorCountryCode = "DE"
        });

        return this;
    }

    public CreateInvoiceDTOFactory CreateClosedInvoice(string currencyCode = "EUR", decimal amount = 1500)
    {
        InvoiceDtos.Add(new CreateInvoiceDTO
        {
            Reference = Guid.NewGuid().ToString(),
            CurrencyCode = currencyCode,
            IssueDate = DateTime.Now.AddDays(-15),
            ClosedDate = DateTime.Now.AddDays(-1),
            OpeningValue = 1500.0m,
            PaidValue = amount,
            DueDate = DateTime.Now.AddDays(45),
            DebtorName = "Jane Doe",
            DebtorReference = "DEB456",
            DebtorCountryCode = "DE"
        });

        return this;
    }

    public CreateInvoiceDTOFactory CreateOverdueInvoice(string currencyCode = "EUR", decimal amount = 1500)
    {
        InvoiceDtos.Add(new CreateInvoiceDTO
        {
            Reference = Guid.NewGuid().ToString(),
            CurrencyCode = currencyCode,
            IssueDate = DateTime.Now.AddDays(-15),
            OpeningValue = 1500.0m,
            PaidValue = amount,
            DueDate = DateTime.Now.AddDays(-5),
            DebtorName = "Jane Doe",
            DebtorReference = "DEB456",
            DebtorCountryCode = "DE"
        });

        return this;
    }

    public CreateInvoiceDTOFactory CreateCancelledInvoice(string currencyCode = "EUR", decimal amount = 1500)
    {
        InvoiceDtos.Add(new CreateInvoiceDTO
        {
            Reference = Guid.NewGuid().ToString(),
            CurrencyCode = currencyCode,
            IssueDate = DateTime.Now.AddDays(-15),
            Cancelled = DateTime.Now.AddDays(-1),
            OpeningValue = 1500.0m,
            PaidValue = amount,
            DueDate = DateTime.Now.AddDays(45),
            DebtorName = "Jane Doe",
            DebtorReference = "DEB456",
            DebtorCountryCode = "DE"
        });

        return this;
    }

    public IList<CreateInvoiceDTO> GetInvoiceDTOs()
    {
        return InvoiceDtos;
    }
}