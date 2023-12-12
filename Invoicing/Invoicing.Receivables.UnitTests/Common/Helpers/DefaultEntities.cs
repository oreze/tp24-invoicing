using Invoicing.Receivables.Domain.Entities;

namespace Invoicing.Receivables.UnitTests.Common.Helpers;

public static class DefaultEntities
{
    public static Invoice GetDefaultInvoice()
    {
        return Invoice.Create(
            "INV001",
            DateTime.Now,
            1000.00m,
            800.00m,
            DateTime.Now.AddDays(30),
            null,
            null,
            GetDefaultDebtor(),
            GetDefaultCurrency()
        );
    }

    public static Debtor GetDefaultDebtor()
    {
        return Debtor.Create("randomDebtor", "DebtorName", null, null, null, null, null, "US", null);
    }

    public static Currency GetDefaultCurrency()
    {
        return Currency.Create("USD", "US Dollar");
    }
}