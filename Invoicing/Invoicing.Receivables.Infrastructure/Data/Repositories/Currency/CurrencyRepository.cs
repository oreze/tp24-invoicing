using Microsoft.EntityFrameworkCore;

namespace Invoicing.Receivables.Infrastructure.Data.Repositories.Currency;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly AppDbContext _dbContext;

    public CurrencyRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<Domain.Entities.Currency> GetByCodeAsync(string code)
    {
        return await _dbContext.Currencies.FirstOrDefaultAsync(c => c.Code == code);
    }

    public async Task AddAsync(Domain.Entities.Currency currency)
    {
        var doesCurrencyExists = await _dbContext.Currencies.AnyAsync(c => c.Code == currency.Code);

        if (!doesCurrencyExists) await _dbContext.Currencies.AddAsync(currency);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}