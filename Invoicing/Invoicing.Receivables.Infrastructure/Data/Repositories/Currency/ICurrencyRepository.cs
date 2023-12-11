namespace Invoicing.Receivables.Infrastructure.Data.Repositories.Currency;

public interface ICurrencyRepository
{
    Task<Domain.Entities.Currency> GetByCodeAsync(string code);
    Task AddAsync(Domain.Entities.Currency currency);
    Task AddRangeAsync(IEnumerable<Domain.Entities.Currency> currency);
    Task<int> SaveChangesAsync();
}
