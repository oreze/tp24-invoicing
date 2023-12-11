using System.Globalization;
using Bogus;
using Invoicing.Receivables.Domain.Entities;
using Invoicing.Receivables.Infrastructure.Data;
using Invoicing.Receivables.Infrastructure.Data.Repositories.Currency;
using Invoicing.Receivables.Infrastructure.Data.Repositories.Debtor;
using Invoicing.Receivables.Infrastructure.Data.Repositories.Invoice;
using Invoicing.Receivables.Infrastructure.Extensions;
using Currency = Invoicing.Receivables.Domain.Entities.Currency;

namespace Invoicing.Receivables.Infrastructure.Seeders;

public class DbSeeder : IDbSeeder
{
    public async Task EnsureSeedDatabase(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var appDbContext = scope.ServiceProvider.GetService<AppDbContext>() ??
                           throw new ArgumentNullException(nameof(AppDbContext));

        if (app.Environment.IsSystemTests() || app.Environment.IsDevelopment()) 
            await appDbContext.Database.EnsureDeletedAsync();

        await appDbContext.Database.EnsureCreatedAsync();

        var currencyRepository = scope.ServiceProvider.GetService<ICurrencyRepository>()
                                 ?? throw new ArgumentNullException(nameof(ICurrencyRepository));
        var debtorRepository = scope.ServiceProvider.GetService<IDebtorRepository>()
                               ?? throw new ArgumentNullException(nameof(IDebtorRepository));
        var invoiceRepository = scope.ServiceProvider.GetService<IInvoiceRepository>()
                                ?? throw new ArgumentNullException(nameof(IInvoiceRepository));

        if (!await appDbContext.IsAnyEntityInDb())
        {
            await SeedInvoices(invoiceRepository, debtorRepository, currencyRepository);
            await appDbContext.SaveChangesAsync();
        }
    }

    private async Task SeedInvoices(IInvoiceRepository invoiceRepository, IDebtorRepository debtorRepository, ICurrencyRepository currencyRepository)
    {
        var faker = new Faker();

        var randomDebtors = await SeedRandomDebtorsAsync(debtorRepository);
        var availableCurrencies = await SeedCurrenciesAsync(currencyRepository);
        var toBeAdded = new List<Invoice>();

        for (var i = 0; i < 1000; i++)
        {
            var issueDate = faker.Date.Past();
            DateTime? closedDate = i % 7 == 0 ? faker.Date.Between(issueDate, DateTime.Today) : null;
            DateTime? cancelled = i % 19 == 0 && !closedDate.HasValue ? faker.Date.Between(issueDate, DateTime.Today) : null;
            var openingValue = Round(faker.Random.Decimal(1000, 5000), 2);
            var paidValue = i % 3 == 0 ? Round(faker.Random.Decimal(0, openingValue), 2) : openingValue;

            var invoice = Invoice.Create(
                $"INV{i:D3}",
                issueDate,
                openingValue,
                paidValue,
                faker.Date.Future(),
                closedDate,
                cancelled,
                randomDebtors[i % 10],
                availableCurrencies.First(x => x.Code == "EUR")
            );

            toBeAdded.Add(invoice);
        }

        await invoiceRepository.AddRangeAsync(toBeAdded);
    }


    private async Task<IList<Currency>> SeedCurrenciesAsync(ICurrencyRepository currencyRepository)
    {
        var currencies = GetCurrenciesList();
        await currencyRepository.AddRangeAsync(currencies);
        
        return currencies;
    }

    private async Task<IList<Debtor>> SeedRandomDebtorsAsync(IDebtorRepository debtorRepository)
    {
        var faker = new Faker();
        var debtors = new List<Debtor>();

        for (var i = 0; i < 10; i++)
        {
            var debtor = Debtor.Create(
                $"DEB{i:D3}",
                faker.Person.Company.Name,
                faker.Address.StreetAddress(),
                faker.Address.SecondaryAddress(),
                faker.Address.City(),
                faker.Address.State(),
                faker.Address.ZipCode(),
                faker.Address.CountryCode(),
                faker.Random.AlphaNumeric(20)
            );

            debtors.Add(debtor);
        }

        await debtorRepository.AddRangeAsync(debtors);
        return debtors;
    }

    private IList<Currency> GetCurrenciesList()
    {
        var regions = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
            .Select(culture => new RegionInfo(culture.Name))
            .Where(IsValidCurrency);

        return regions
            .Select(region => Currency.Create(region.ISOCurrencySymbol, region.CurrencyEnglishName))
            .DistinctBy(currency => currency.Code)
            .ToList();
    }

    private bool IsValidCurrency(RegionInfo region)
    {
        return !string.IsNullOrWhiteSpace(region.ISOCurrencySymbol) && !string.IsNullOrWhiteSpace(region.CurrencyEnglishName);
    }

    private decimal Round(decimal number, int decimals)
    {
        return Math.Round(number, decimals);
    }
}
